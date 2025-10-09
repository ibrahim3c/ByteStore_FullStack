using ByteStore.Application.DTOs.Category;
using ByteStore.Domain.Abstractions.Result;
using ByteStore.Domain.Entities;
using ByteStore.Domain.Repositories;
using BytStore.Application.DTOs.Category;
using BytStore.Application.IServices;


namespace BytStore.Application.Services
{
    public class CategoryService:ICategoryService
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result2<IEnumerable<CategoryDto>>> GetAllCategoriesAsync()
        {
            var categories = await unitOfWork.CategoryRepository.GetAllAsync();
            var categoriesDto = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });
            return Result2<IEnumerable<CategoryDto>>.Success(categoriesDto);
        }

        public async Task<Result2<IEnumerable<CategoryTreeDto>>> GetAllCategoryTreesAsync()
        {
            var categories = await unitOfWork.CategoryRepository.GetAllAsync(["SubCategories"]);
            var topLevelCategories = categories.Where(c => c.ParentCategoryId == null).ToList();
            var categoryDtos = topLevelCategories.Select(MapToDto).ToList();
            return Result2<IEnumerable<CategoryTreeDto>>.Success(categoryDtos);
        }
        private CategoryTreeDto MapToDto(Category category)
        {
            return new CategoryTreeDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                SubCategories = category.SubCategories?.Select(MapToDto).ToList()
            };
        }

        public async Task<Result2<CategoryDto>> GetCategoryByIdAsync(int categoryId)
        {
            var category = await unitOfWork.CategoryRepository.GetByIdAsync(categoryId);
            if (category == null)
                return Result2<CategoryDto>.Failure(CategoryErrors.CategoryNotFound);
            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
            return Result2<CategoryDto>.Success(categoryDto);
        }
        public async Task<Result2> CreateCategoryAsync(CategoryDto categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description
            };
            await unitOfWork.CategoryRepository.AddAsync(category);
            await unitOfWork.SaveChangesAsync();
            return Result2.Success();
        }
        public async Task<Result2> UpdateCategoryAsync(int categoryId, CategoryDto categoryDto)
        {
            var category = await unitOfWork.CategoryRepository.GetByIdAsync(categoryId);
            if (category == null)
                return Result2.Failure(CategoryErrors.CategoryNotFound);
            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;
            unitOfWork.CategoryRepository.Update(category);
            await unitOfWork.SaveChangesAsync();
            return Result2.Success();
        }

        public async Task<Result2> DeleteCategoryAsync(int categoryId)
        {
            var category = await unitOfWork.CategoryRepository.GetByIdAsync(categoryId);
            if (category == null)
                return Result2.Failure(CategoryErrors.CategoryNotFound);
            unitOfWork.CategoryRepository.Delete(category);
            await unitOfWork.SaveChangesAsync();
            return Result2.Success();
        }

    }
}
