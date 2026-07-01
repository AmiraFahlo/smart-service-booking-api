namespace SmartServiceBooking.Application.Common.Interfaces;

public interface IJwtProvider
{
    AuthResult Generate(ApplicationUser user, IEnumerable<string> roles);
}
