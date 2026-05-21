using Microsoft.AspNetCore.Identity;

namespace SmartServiceBooking.Domain.Entities.Identity;

public class ApplicationUserToken : IdentityUserToken<int>
{
    public ApplicationUser User { get; set; } = default!;
}
