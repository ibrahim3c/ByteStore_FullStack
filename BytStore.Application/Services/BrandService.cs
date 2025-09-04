using ByteStore.Domain.Abstractions.Result;
using ByteStore.Domain.Entities;
using ByteStore.Domain.Repositories;
using BytStore.Application.DTOs.Brand;
using BytStore.Application.IServices;
using MyResult = ByteStore.Domain.Abstractions.Result.Result;

namespace BytStore.Application.Services
{
    public class BrandService:IBrandService
    {
        private readonly IUnitOfWork unitOfWork;
        public BrandService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<BrandDto>>> GetAllBrandsAsync()
        {
            var brands = await unitOfWork.GetRepository<Brand>().GetAllAsync();
            var brandsDto = brands.Select(c => new BrandDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });
            return Result<IEnumerable<BrandDto>>.Success(brandsDto);
        }
        public async Task<Result<BrandDto>> GetBrandByIdAsync(int brandId)
        {
            var category = await unitOfWork.GetRepository<Brand>().GetByIdAsync(brandId);
            if (category == null)
                return Result<BrandDto>.Failure(["Brand not found"]);
            var categoryDto = new BrandDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
            return Result<BrandDto>.Success(categoryDto);
        }
        public async Task<MyResult> CreateBrandAsync(BrandDto brandDto)
        {
            // validate dto
            var brand = new Brand
            {
                Name = brandDto.Name,
                Description = brandDto.Description
            };
            await unitOfWork.GetRepository<Brand>().AddAsync(brand);
            await unitOfWork.SaveChangesAsync();
            return MyResult.Success();
        }
        public async Task<MyResult> UpdateBrandAsync(int brandId, BrandDto brandDto)
        {
            // validate dto
            var brand = await unitOfWork.GetRepository<Brand>().GetByIdAsync(brandId);
            if (brand == null)
                return MyResult.Failure(["brand not found"]);
            brand.Name = brandDto.Name;
            brand.Description = brandDto.Description;
            unitOfWork.GetRepository<Brand>().Update(brand);
            await unitOfWork.SaveChangesAsync();
            return MyResult.Success();
        }
        public async Task<MyResult> DeleteBrandAsync(int brandId)
        {
            var brand = await unitOfWork.GetRepository<Brand>().GetByIdAsync(brandId);
            if (brand == null)
                return MyResult.Failure(["Brand not found"]);
            unitOfWork.GetRepository<Brand>().Delete(brand);
            await unitOfWork.SaveChangesAsync();
            return MyResult.Success();
        }


    }
}
