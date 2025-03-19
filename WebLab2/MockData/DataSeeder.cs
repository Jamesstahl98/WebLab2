using Microsoft.AspNetCore.Identity;
using WebLab2.Data;
using WebLab2.Entities;

namespace WebLab2.MockData;

public class DataSeeder
{
    public static async Task SeedUsers(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        await context.Database.EnsureCreatedAsync();

        if (!context.Users.Any(u => u.Role == "Admin"))
        {
            var adminUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "admin",
                FirstName = "Admin",
                LastName = "Adminson",
                Email = "admin@admin.com",
                PhoneNumber = "123456789",
                Country = "Adminland",
                City = "Admincity",
                Address = "Adminstreet 123",
                Role = "Admin"
            };
            adminUser.PasswordHash = new PasswordHasher<User>().HashPassword(adminUser, "Admin123!");

            context.Users.Add(adminUser);
        }

        if (!context.Users.Any(u => u.Role == "User"))
        {
            var regularUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "user",
                FirstName = "User",
                LastName = "Userson",
                Email = "user@user.com",
                PhoneNumber = "987654321",
                Country = "Userland",
                City = "Usercity",
                Address = "Userstreet 123",
                Role = "User"
            };
            regularUser.PasswordHash = new PasswordHasher<User>().HashPassword(regularUser, "User123!");

            context.Users.Add(regularUser);
        }

        await context.SaveChangesAsync();
    }
}
