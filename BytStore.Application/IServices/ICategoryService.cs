using ByteStore.Domain.Abstractions.Result;
using BytStore.Application.DTOs.Category;
using MyResult = ByteStore.Domain.Abstractions.Result.Result;
namespace BytStore.Application.IServices
{
    public interface ICategoryService
    {
        Task<Result<IEnumerable<CategoryDto>>> GetAllCategoriesAsync();
        Task<Result<CategoryDto>> GetCategoryByIdAsync(int categoryId);
        Task<MyResult> CreateCategoryAsync(CategoryDto categoryDto);
        Task<MyResult> UpdateCategoryAsync(int categoryId, CategoryDto categoryDto);
        Task<MyResult> DeleteCategoryAsync(int categoryId);
    }
}
