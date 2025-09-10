using ByteStore.Domain.Abstractions.Result;
using BytStore.Application.DTOs.Brand;
using MyResult = ByteStore.Domain.Abstractions.Result.Result;

namespace BytStore.Application.IServices
{
    public interface IBrandService
    {
        Task<Result2<IEnumerable<BrandDto>>> GetAllBrandsAsync();
        Task<Result2<BrandDto>> GetBrandByIdAsync(int brandId);
        Task<Result2> CreateBrandAsync(BrandDto brandDto);
        Task<Result2> UpdateBrandAsync(int brandId, BrandDto brandDto);
        Task<Result2> DeleteBrandAsync(int brandId);
    }
}
