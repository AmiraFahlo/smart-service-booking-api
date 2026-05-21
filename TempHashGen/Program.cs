using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using SmartServiceBooking.Domain.Entities.Identity;
using System;

public class Program
{
    public static void Main()
    {
        var services = new ServiceCollection();
        
        // Add Identity defaults
        services.AddIdentity<ApplicationUser, ApplicationRole>();
        
        var provider = services.BuildServiceProvider();
        var hasher = provider.GetRequiredService<IPasswordHasher<ApplicationUser>>();
        
        var user = new ApplicationUser { UserName = "p1@test.com", Email = "p1@test.com" }; 
        
        var hash = hasher.HashPassword(user, "manual");
        Console.WriteLine("HASH:" + hash);
    }
}