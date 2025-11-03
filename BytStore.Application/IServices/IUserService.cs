using ByteStore.Domain.Abstractions.Result;
using BytStore.Application.DTOs.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BytStore.Application.IServices
{
    public interface IUserService
    {
        Task<Result2<UserDto>> GetCurrentUserAsync(ClaimsPrincipal userPrincipal);
        Task<Result2<IEnumerable<UserDto>>> GetAllUsersAsync();
        Task<Result2<UserDto>> GetUserByIdAsync(string userID);
        Task<Result2<UserDto>> GetUserByEmailAsync(string email);
        Task<Result2<IEnumerable<GetRoleDto>>> GetRolesOfUserAsync(string userId);
        Task<Result2<IEnumerable<string>>> GetRolesNameOfUserAsync(string userId);
        Task<Result2> LockUnLock(string id);
        Task<Result2> AddUserAsync(CreatedUserDto createdUserDto);
        Task<Result2> UpdateUserAsync(UpdateUserDto userDTO);
        Task<Result2> DeleteUserAsync(string userID);
        Task<Result2> DeleteUserByEmailAsync(string email);
        Task<Result2<ManageRolesDto>> GetRolesForManagingAsync(string userId);
        Task<Result2> ManageUserRolesAsync(ManageRolesDto manageRolesDTO);

    }
}
