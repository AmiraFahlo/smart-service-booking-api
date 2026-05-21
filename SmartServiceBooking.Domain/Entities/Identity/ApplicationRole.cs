namespace SmartServiceBooking.Domain.Entities.Identity;

public class ApplicationRole : IdentityRole<int>
{
    public string? Description { get; set; }
    public HashSet<ApplicationUserRole> UserRoles { get; set; } = new();
}
