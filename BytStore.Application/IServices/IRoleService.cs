using ByteStore.Domain.Abstractions.Result;
using BytStore.Application.DTOs.Identity;

namespace BytStore.Application.IServices
{
    public interface IRolesService
    {
        Task<Result2<IEnumerable<GetRoleDto>>> GetAllRolesAsync();
        Task<Result2<GetRoleDto>> GetRoleByIdAsync(string roleId);
        Task<Result2> AddRoleAsync(GetRoleDto roleDTO);
        Task<Result2> UpdateRoleAsync(GetRoleDto roleDTO);
        Task<Result2> DeleteRoleAsync(string roleId);
    }
}
