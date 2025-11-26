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
        private readonly IImageService imageService;

        public CategoryService(IUnitOfWork unitOfWork, IImageService imageService)
        {
            this.unitOfWork = unitOfWork;
            this.imageService = imageService;
        }


        public async Task<Result2<IEnumerable<CategoryDto>>> GetAllCategoriesAsync()
        {
            var categories = await unitOfWork.GetRepository<Category>().GetAllAsync();
            var categoriesDto = categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                ImageUrl=c.ImageUrl
                
            });
            return Result2<IEnumerable<CategoryDto>>.Success(categoriesDto);
        }
        public async Task<Result2<CategoryDto>> GetCategoryByIdAsync(int categoryId)
        {
            var category = await unitOfWork.GetRepository<Category>().GetByIdAsync(categoryId);
            if (category == null)
                return Result2<CategoryDto>.Failure(CategoryErrors.CategoryNotFound);
            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ImageUrl = category.ImageUrl

            };
            return Result2<CategoryDto>.Success(categoryDto);
        }
        public async Task<Result2> CreateCategoryAsync(CategoryForCreateDto categoryDto)
        {
            var result = await imageService.UploadAsync(categoryDto.Image, "categories");
            if (!result.IsSuccess)
                return Result2.Failure(result.Error);

            var category = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                ImageUrl=result.Value.Url
            };
            await unitOfWork.GetRepository<Category>().AddAsync(category);
            await unitOfWork.SaveChangesAsync();
            return Result2.Success();
        }


        public async Task<Result2> UpdateCategoryAsync(int categoryId, CategoryForCreateDto categoryDto)
        {
            var category = await unitOfWork.GetRepository<Category>().GetByIdAsync(categoryId);
            if (category == null)
                return Result2.Failure(CategoryErrors.CategoryNotFound);
            var result=await imageService.UpdateImageAsync(category.ImageUrl, categoryDto.Image);
            if (!result.IsSuccess)
                return Result2.Failure(result.Error);

            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;
            category.ImageUrl = result.Value;

            unitOfWork.GetRepository<Category>().Update(category);
            await unitOfWork.SaveChangesAsync();
            return Result2.Success();
        }




        public async Task<Result2> DeleteCategoryAsync(int categoryId)
        {
            var category = await unitOfWork.GetRepository<Category>().GetByIdAsync(categoryId);
            if (category == null)
                return Result2.Failure(CategoryErrors.CategoryNotFound);
            unitOfWork.GetRepository<Category>().Delete(category);
           var result= await imageService.DeleteAsync(category.ImageUrl);
            if (!result.IsSuccess)
                return Result2.Failure(result.Error);
            await unitOfWork.SaveChangesAsync();

            return Result2.Success();
        }


        public async Task<Result2<IEnumerable<CategoryTreeDto>>> GetAllCategoryTreesAsync()
        {
            var categories = await unitOfWork.CategoryTreeRepository.GetAllAsync(["SubCategories"]);
            var topLevelCategories = categories.Where(c => c.ParentCategoryId == null).ToList();
            var categoryDtos = topLevelCategories.Select(MapToDto).ToList();
            return Result2<IEnumerable<CategoryTreeDto>>.Success(categoryDtos);
        }
        private CategoryTreeDto MapToDto(CategoryTree category)
        {
            return new CategoryTreeDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                SubCategories = category.SubCategories?.Select(MapToDto).ToList()
            };
        }

    }
}
