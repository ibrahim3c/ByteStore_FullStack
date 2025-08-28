using ByteStore.Domain.Abstractions;
using ByteStore.Domain.Entities;
using ByteStore.Domain.Repositories;
using BytStore.Application.DTOs;

namespace BytStore.Application.Services
{
    public class CategoryService
    {
        private readonly IUnitOfWork unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<IEnumerable<CategoryDto>>> GetAllCategoriesAsync()
        {
            var categories = await unitOfWork.CategoryRepository.GetAllAsync();
            var categoriesDto = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });
            return Result<IEnumerable<CategoryDto>>.Success(categoriesDto);
        }
        public async Task<Result<CategoryDto>> GetCategoryByIdAsync(int categoryId)
        {
            var category = await unitOfWork.CategoryRepository.GetByIdAsync(categoryId);
            if (category == null)
                return Result<CategoryDto>.Failure(["Category not found"]);
            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
            return Result<CategoryDto>.Success(categoryDto);
        }
        public async Task<Result> CreateCategoryAsync(CategoryDto categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description
            };
            await unitOfWork.CategoryRepository.AddAsync(category);
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }
        public async Task<Result> UpdateCategoryAsync(int categoryId, CategoryDto categoryDto)
        {
            var category = await unitOfWork.CategoryRepository.GetByIdAsync(categoryId);
            if (category == null)
                return Result.Failure(["Category not found"]);
            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;
            unitOfWork.CategoryRepository.Update(category);
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> DeleteCategoryAsync(int categoryId)
        {
            var category = await unitOfWork.CategoryRepository.GetByIdAsync(categoryId);
            if (category == null)
                return Result.Failure(["Category not found"]);
            unitOfWork.CategoryRepository.Delete(category);
            await unitOfWork.SaveChangesAsync();
            return Result.Success();
        }

    }
}
