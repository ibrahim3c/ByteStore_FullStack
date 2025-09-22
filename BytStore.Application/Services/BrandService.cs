using ByteStore.Domain.Abstractions.Result;
using ByteStore.Domain.Entities;
using ByteStore.Domain.Repositories;
using BytStore.Application.DTOs.Brand;
using BytStore.Application.IServices;

namespace BytStore.Application.Services
{
    public class BrandService:IBrandService
    {
        private readonly IUnitOfWork unitOfWork;
        public BrandService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result2<IEnumerable<BrandDto>>> GetAllBrandsAsync()
        {
            var brands = await unitOfWork.GetRepository<Brand>().GetAllAsync();
            var brandsDto = brands.Select(c => new BrandDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });
            return Result2<IEnumerable<BrandDto>>.Success(brandsDto);
        }
        public async Task<Result2<BrandDto>> GetBrandByIdAsync(int brandId)
        {
            var category = await unitOfWork.GetRepository<Brand>().GetByIdAsync(brandId);
            if (category == null)
                return Result2<BrandDto>.Failure(BrandErrors.BrandNotFound);
            var categoryDto = new BrandDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
            return Result2<BrandDto>.Success(categoryDto);
        }
        public async Task<Result2> CreateBrandAsync(BrandDto brandDto)
        {
            // validate dto
            var brand = new Brand
            {
                Name = brandDto.Name,
                Description = brandDto.Description
            };
            await unitOfWork.GetRepository<Brand>().AddAsync(brand);
            await unitOfWork.SaveChangesAsync();
            return Result2.Success();
        }
        public async Task<Result2> UpdateBrandAsync(int brandId, BrandDto brandDto)
        {
            // validate dto
            var brand = await unitOfWork.GetRepository<Brand>().GetByIdAsync(brandId);
            if (brand == null)
                return Result2.Failure(BrandErrors.BrandNotFound);
            brand.Name = brandDto.Name;
            brand.Description = brandDto.Description;
            unitOfWork.GetRepository<Brand>().Update(brand);
            await unitOfWork.SaveChangesAsync();
            return Result2.Success();
        }
        public async Task<Result2> DeleteBrandAsync(int brandId)
        {
            var brand = await unitOfWork.GetRepository<Brand>().GetByIdAsync(brandId);
            if (brand == null)
                return Result2.Failure(BrandErrors.BrandNotFound);
            unitOfWork.GetRepository<Brand>().Delete(brand);
            await unitOfWork.SaveChangesAsync();
            return Result2.Success();
        }


    }
}
