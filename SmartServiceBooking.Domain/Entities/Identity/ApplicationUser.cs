namespace SmartServiceBooking.Domain.Entities.Identity;
public class ApplicationUser : IdentityUser<int>
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;

    // Auditing
    public DateTime CreatedAt { get; set; }

    public bool IsDeleted { get; set; }

    // Profile
    public bool IsActive { get; set; } = true;


    // Refresh tokens
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    // Navigation properties
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = new List<ApplicationUserRole>();
    public virtual ICollection<ApplicationUserToken> UserTokens { get; set; } = new List<ApplicationUserToken>();
}
