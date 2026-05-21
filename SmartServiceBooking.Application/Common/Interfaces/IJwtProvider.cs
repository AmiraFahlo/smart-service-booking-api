using SmartServiceBooking.Domain.Entities.Identity;

using SmartServiceBooking.Application.Features.Auth;

namespace SmartServiceBooking.Application.Common.Interfaces;

public interface IJwtProvider
{
    AuthResult Generate(ApplicationUser user, IEnumerable<string> roles);
}
