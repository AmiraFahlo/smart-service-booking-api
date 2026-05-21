using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SmartServiceBooking.Infrastructure.Identity.Options;
using SmartServiceBooking.Application.Features.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartServiceBooking.Infrastructure.Identity.Services;

public class JwtProvider(IOptions<JwtOptions> jwtOptions) : IJwtProvider
{
    private readonly JwtOptions _options = jwtOptions.Value;

    public AuthResult Generate(ApplicationUser user, IEnumerable<string> roles)
    {
        // 1. Generate JWT (Access Token)
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email!),
            new("fullName", $"{user.FirstName} {user.LastName}")
        };

        foreach (var role in roles)
        {
            claims.Add(new(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            null,
            DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes),
            creds
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        // 2. Generate Refresh Token
        var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_options.RefreshTokenExpiryDays);

        return new AuthResult(accessToken, refreshToken, refreshTokenExpiration);
    }
}
