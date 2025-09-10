using ByteStore.Domain.Abstractions.Result;
using BytStore.Application.DTOs.Category;
using MyResult = ByteStore.Domain.Abstractions.Result.Result;
namespace BytStore.Application.IServices
{
    public interface ICategoryService
    {
        Task<Result2<IEnumerable<CategoryDto>>> GetAllCategoriesAsync();
        Task<Result2<CategoryDto>> GetCategoryByIdAsync(int categoryId);
        Task<Result2> CreateCategoryAsync(CategoryDto categoryDto);
        Task<Result2> UpdateCategoryAsync(int categoryId, CategoryDto categoryDto);
        Task<Result2> DeleteCategoryAsync(int categoryId);
    }
}
