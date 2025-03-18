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
                Role = "User"
            };
            regularUser.PasswordHash = new PasswordHasher<User>().HashPassword(regularUser, "User123!");

            context.Users.Add(regularUser);
        }

        await context.SaveChangesAsync();
    }
}
