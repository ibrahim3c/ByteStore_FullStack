using ByteStore.Domain.Abstractions.Constants;
using ByteStore.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ByteStore.Persistance.Seeders
{
    public class RoleSeeder
    {
        public static  async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();

            foreach (var roleName in Roles.AllRoles)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new AppRole { Name = roleName });
                }
            }
        }
    }
}
