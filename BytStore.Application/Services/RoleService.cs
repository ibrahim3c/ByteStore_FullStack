using ByteStore.Domain.Abstractions.Result;
using ByteStore.Domain.Entities;
using BytStore.Application.DTOs.Identity;
using BytStore.Application.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BytStore.Application.Services
{
    public class RoleService : IRolesService
    {
        private readonly RoleManager<AppRole> roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        // get
        public async Task<Result2<IEnumerable<GetRoleDto>>> GetAllRolesAsync()
        {
            var roles = await roleManager.Roles.Select(r => new GetRoleDto
            {
                RoleId = r.Id,
                RoleName = r.Name
            }).ToListAsync();

            if (!roles.Any())
                return Result2<IEnumerable<GetRoleDto>>.Failure(RoleErrors.NotFound);

            return Result2<IEnumerable<GetRoleDto>>.Success(roles);
        }
        public async Task<Result2<GetRoleDto>> GetRoleByIdAsync(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
                return Result2<GetRoleDto>.Failure(RoleErrors.NotFound);

            var roleDTO = new GetRoleDto { RoleId = roleId, RoleName = role.Name };

            return Result2<GetRoleDto>.Success(roleDTO);
        }

        // add
        public async Task<Result2> AddRoleAsync(GetRoleDto roleDTO)
        {
            if (await roleManager.FindByNameAsync(roleDTO.RoleName) != null)
            {
                return Result2.Failure(RoleErrors.AlreadyExists);
            }

            var role = new AppRole
            {
                Name = roleDTO.RoleName
            };

            IdentityResult result = await roleManager.CreateAsync(role);

            if (result.Succeeded)
                return Result2<GetRoleDto>.Success();

            return Result2<GetRoleDto>.Failure(RoleErrors.CreateFailed);
        }

        // update
        public async Task<Result2> UpdateRoleAsync(GetRoleDto roleDTO)
        {
            var role = await roleManager.FindByIdAsync(roleDTO.RoleId);
            if (role == null)
                return Result2.Failure(RoleErrors.NotFound);

            if (role.Name != roleDTO.RoleName)
            {
                role.Name = roleDTO.RoleName;
                IdentityResult result = await roleManager.UpdateAsync(role);

                if (!result.Succeeded)
                    return Result2<GetRoleDto>.Failure(RoleErrors.UpdateFailed);
            }

            return Result2.Success();
        }

        // delete role
        public async Task<Result2> DeleteRoleAsync(string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);
            if (role == null)
                return Result2.Failure(RoleErrors.NotFound);

            IdentityResult result = await roleManager.DeleteAsync(role);

            if (!result.Succeeded)
                return Result2.Failure(RoleErrors.DeleteFailed);

            return Result2.Success();



        }
    }
}
