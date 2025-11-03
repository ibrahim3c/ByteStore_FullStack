using ByteStore.Domain.Abstractions.Result;
using ByteStore.Domain.Entities;
using BytStore.Application.DTOs.Identity;
using BytStore.Application.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BytStore.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;
        private readonly IRolesService rolesService;

        public UserService(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            IRolesService rolesService)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.rolesService = rolesService;
        }

        // get
        public async Task<Result2<UserDto>> GetCurrentUserAsync(ClaimsPrincipal userPrincipal)
        {
            var userId = userManager.GetUserId(userPrincipal);

            if (userId == null)
                return Result2<UserDto>.Failure(UserErrors.NotFound);

            var user = await userManager.FindByIdAsync(userId);

            if (user == null)
                return Result2<UserDto>.Failure(UserErrors.NotFound);

            var userDto = new UserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
            return Result2<UserDto>.Success(userDto);
        }

        public async Task<Result2<IEnumerable<UserDto>>> GetAllUsersAsync()
        {
            var users = await userManager.Users.Select(u => new UserDto
            {
                UserName = u.UserName,
                Email = u.Email,
                PhoneNumber = u.PhoneNumber,
            }).ToListAsync();
            if (!users.Any())
                return Result2<IEnumerable<UserDto>>.Failure(UserErrors.NotFound);

            return Result2<IEnumerable<UserDto>>.Success(users);
        }
        public async Task<Result2<UserDto>> GetUserByIdAsync(string userID)
        {
            var user = await userManager.FindByIdAsync(userID);
            if (user == null)
                return Result2<UserDto>.Failure(UserErrors.NotFound);
            var userDto = new UserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };

            return Result2<UserDto>.Success( userDto);
        }

        public async Task<Result2<IEnumerable<GetRoleDto>>> GetRolesOfUserAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return Result2<IEnumerable<GetRoleDto>>.Failure(UserErrors.NotFound);

            var rolesNames = await userManager.GetRolesAsync(user);
            if (!rolesNames.Any())
                return Result2<IEnumerable<GetRoleDto>>.Failure(RoleErrors.NotFound);

            // if u want to get roles as object
            var roles = new List<GetRoleDto>();
            foreach (var roleName in rolesNames)
            {
                var role = await roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    roles.Add(new GetRoleDto
                    {
                        RoleId = role.Id,
                        RoleName = roleName,
                    });
                }
            }

            return Result2<IEnumerable<GetRoleDto>>.Success(roles);
        }
        public async Task<Result2<UserDto>> GetUserByEmailAsync(string email)
        {

            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return Result2<UserDto>.Failure(UserErrors.NotFound);
            var userDto = new UserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };

            return Result2<UserDto>.Success(userDto);
        }

        // lock
        public async Task<Result2>LockUnLock(string id)
        {
            // Find the user by their Id
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
                return Result2.Failure(UserErrors.NotFound);

            // If user is not locked out, lock them for 1 year
            if (user.LockoutEnd == null || user.LockoutEnd < DateTime.UtcNow)
            {
                user.LockoutEnd = DateTime.UtcNow.AddYears(1);  // Lock the user
            }
            else
            {
                // Unlock the user by setting LockoutEnd to null
                user.LockoutEnd = null;
            }

            // Update the user in the database
            await userManager.UpdateAsync(user);

            return Result2.Success();
        }

        // add
        public async Task<Result2> AddUserAsync(CreatedUserDto createduserDto)
        {
            if (await userManager.FindByEmailAsync(createduserDto.Email) is not null)
                return Result2.Failure(UserErrors.RequiredEmail);

            var user = new AppUser
            {
                UserName = createduserDto.FirstName + createduserDto.LastName,  // Use email as username
                Email = createduserDto.Email,
                PhoneNumber = createduserDto.PhoneNumber,
            };

            var result = await userManager.CreateAsync(user, createduserDto.Password);
            if (!result.Succeeded)
                return Result2.Failure(UserErrors.AddFailed);

            return Result2<string>.Success(user.Id);
        }
        // update 
        public async Task<Result2> UpdateUserAsync(UpdateUserDto userDto)
        {
            var user = await userManager.FindByIdAsync(userDto.UserId);
            if (user == null)
                return Result2.Failure(UserErrors.NotFound);

            if (user.UserName != userDto.UserName)
                user.UserName = userDto.UserName;


            if (user.PhoneNumber != userDto.PhoneNumber)
                user.PhoneNumber = userDto.PhoneNumber;

            await userManager.UpdateAsync(user);

            return Result2.Success();

        }


        // delete User
        public async Task<Result2> DeleteUserAsync(string userID)
        {
            var user = await userManager.FindByIdAsync(userID);

            if (user == null)
                return Result2.Failure(UserErrors.NotFound);

            IdentityResult result = await userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return Result2.Failure(UserErrors.DeleteFailed);
            }

            return Result2.Success();
        }

        public async Task<Result2> DeleteUserByEmailAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
                return Result2.Failure(UserErrors.NotFound);

            IdentityResult result = await userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return Result2.Failure(UserErrors.DeleteFailed);

            }
            return Result2.Success();

        }


        // User Roles
        public async Task<Result2<IEnumerable<string>>> GetRolesNameOfUserAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return Result2<IEnumerable<string>>.Failure(UserErrors.NotFound);

            var roles = await userManager.GetRolesAsync(user);
            if (!roles.Any())
                return Result2<IEnumerable<string>>.Failure(RoleErrors.NotFound);

            return Result2<IEnumerable<string>>.Success(roles);
        }
        public async Task<Result2<ManageRolesDto>> GetRolesForManagingAsync(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
                return Result2<ManageRolesDto>.Failure(UserErrors.NotFound);

            var roles = await rolesService.GetAllRolesAsync();

            if (!roles.IsSuccess)
                return Result2<ManageRolesDto>.Failure(roles.Error);

            var manageRoles = roles.Value.Select(r => new RolesDto
            {
                RoleName = r.RoleName,
                IsSelected = userManager.IsInRoleAsync(user, r.RoleName).Result
            }).ToList();

            var UserRoles = new ManageRolesDto
            {
                Roles = manageRoles,
                UserId = userId
            };
            return Result2<ManageRolesDto>.Success( UserRoles);

        }
        public async Task<Result2> ManageUserRolesAsync(ManageRolesDto manageRolesDto)
        {
            var user = await userManager.FindByIdAsync(manageRolesDto.UserId);

            if (user == null)
                return Result2.Failure(UserErrors.NotFound);

            var userRoles = await userManager.GetRolesAsync(user);

            foreach (var role in manageRolesDto.Roles)
            {
                if (userRoles.Any(r => r == role.RoleName) && !role.IsSelected)
                    await userManager.RemoveFromRoleAsync(user, role.RoleName);

                if (!userRoles.Any(r => r == role.RoleName) && role.IsSelected)
                    await userManager.AddToRoleAsync(user, role.RoleName);
            }

            return Result2.Success();
        }
    }
}
