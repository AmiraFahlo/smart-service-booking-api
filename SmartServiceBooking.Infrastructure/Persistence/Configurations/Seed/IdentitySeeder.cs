namespace SmartServiceBooking.Infrastructure.Persistence.Configurations.Seed;
public static class IdentitySeeder
{
    public static async Task SeedAsync(
         UserManager<ApplicationUser> userManager,
         RoleManager<ApplicationRole> roleManager,
         ApplicationDbContext context) 
    {
        var strategy = context.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                await EnsureRole(roleManager, "Admin");
                await EnsureRole(roleManager, "Customer");
                await EnsureRole(roleManager, "Provider");

                await EnsureUser(
                    userManager,
                    email: "admin@smartbooking.com",
                    password: "Password123!",
                    role: "Admin",
                    firstName: "System",
                    lastName: "Admin"
                );

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        });
    }

    private static async Task EnsureRole(
        RoleManager<ApplicationRole> roleManager,
        string role)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new ApplicationRole { Name = role });
    }

    private static async Task EnsureUser(
        UserManager<ApplicationUser> userManager,
        string email,
        string password,
        string role,
        string firstName,
        string lastName)
    {
        var user = await userManager.FindByEmailAsync(email);
        
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FirstName = firstName,
                LastName = lastName,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        // Ensure user is in role
        if (!await userManager.IsInRoleAsync(user, role))
        {
            await userManager.AddToRoleAsync(user, role);
        }
    }
}
