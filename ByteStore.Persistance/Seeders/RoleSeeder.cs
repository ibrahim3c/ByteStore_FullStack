using ByteStore.Domain.Abstractions.Constants;
using ByteStore.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ByteStore.Persistance.Seeders
{
    public class RoleSeeder
    {
        private readonly RoleManager<AppRole> roleManager;

        public RoleSeeder(RoleManager<AppRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        public async Task SeedAsync()
        {
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
