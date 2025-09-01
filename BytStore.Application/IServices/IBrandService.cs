using ByteStore.Domain.Abstractions;
using BytStore.Application.DTOs.Brand;
using MyResult = ByteStore.Domain.Abstractions.Result;

namespace BytStore.Application.IServices
{
    public interface IBrandService
    {
        Task<Result<IEnumerable<BrandDto>>> GetAllBrandsAsync();
        Task<Result<BrandDto>> GetBrandByIdAsync(int brandId);
        Task<MyResult> CreateBrandAsync(BrandDto brandDto);
        Task<MyResult> UpdateBrandAsync(int brandId, BrandDto brandDto);
        Task<MyResult> DeleteBrandAsync(int brandId);
    }
}
