using App.Entities;
using Microsoft.AspNetCore.Identity;

namespace App.DbSeeder;

public static class AuthDbContextSeed
{
    public static async Task SeedAdminAndTestUsers(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<AuthUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        
        if (!await roleManager.RoleExistsAsync("Admin"))
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        
        var adminUser = await userManager.FindByEmailAsync("admin@mail.com");
        if (adminUser == null)
        {
            adminUser = new AuthUser
            {
                UserName = "Admin",
                Email = "admin@mail.com",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(adminUser, "Admin1!");
            
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
        
        var testUser = await userManager.FindByEmailAsync("test@mail.com");
        if (testUser == null)
        {
            testUser = new AuthUser
            {
                UserName = "Test",
                Email = "test@mail.com",
                EmailConfirmed = true
            };
            await userManager.CreateAsync(testUser, "Test1!");
        }
    }
}
