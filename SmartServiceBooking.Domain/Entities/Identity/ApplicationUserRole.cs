using Microsoft.AspNetCore.Identity;

namespace SmartServiceBooking.Domain.Entities.Identity;

public class ApplicationUserRole : IdentityUserRole<int>
{
    public ApplicationUser User { get; set; } = default!;
    public ApplicationRole Role { get; set; } = default!;
}
