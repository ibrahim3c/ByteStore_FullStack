using ByteStore.Domain.Abstractions.Constants;
using ByteStore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ByteStore.Persistance.Seeders
{
    public class AdminSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();

            // Ensure Role exists
            string adminRole = Roles.AdminRole;
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new AppRole
                {
                    Name = adminRole
                });
            }

            // Create admin user if not exists
            string adminEmail = "admin@gmail.com";
            string adminPassword = "Test1234+";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = "Admin",
                    Email = adminEmail,
                    EmailConfirmed = true,
                    PhoneNumber = "01011283465",
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
            }
        }

    }
}
