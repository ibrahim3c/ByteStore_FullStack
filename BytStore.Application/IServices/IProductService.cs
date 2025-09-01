using ByteStore.Domain.Abstractions;
using BytStore.Application.DTOs.Product;
using MyResult = ByteStore.Domain.Abstractions.Result;

namespace BytStore.Application.IServices
{
    public interface IProductService
    {
        // Product operations
        Task<Result<IEnumerable<ProductListDto>>> GetAllProductsAsync();
        Task<Result<PagedDto<ProductListDto>>> GetAllProductsAsync(int pageNumber, int pageSize);
        Task<ProductDetailsDto> GetProductByIdAsync(int productId);
        Task<Result<IEnumerable<ProductListDto>>> GetProductsByCategoryIdAsync(int categoryId);
        Task<Result<IEnumerable<ProductListDto>>> GetProductsByBrandIdAsync(int brandId);
        Task<Result<IEnumerable<ProductListDto>>> SearchProductsAsync(string query);
        Task<Result<int>> AddProductAsync(ProductCreateDto productCreateDto);
        Task<MyResult> UpdateProductAsync(int productId, ProductUpdateDto productUpdateDto);
        Task<MyResult> DeleteProductAsync(int productId);

        // Product image operations
        Task<Result<IEnumerable<ProductImageDto>>> GetProductImagesAsync(int productId);
        Task<Result<int>> AddProductImagesAsync(int productId, List<ProductImageCreateDto> productImageCreateDtos);
        Task<MyResult> DeleteProductImageAsync(int productImageId);
        Task<MyResult> SetPrimaryProductImageAsync(int productImageId);

        // Product review operations
        Task<Result<int>> AddProductReviewAsync(int productId, ProductReviewCreateDto productReviewCreateDto);
        Task<MyResult> UpdateProductReviewAsync(int reviewId, ProductReviewUpdateDto productReviewUpdateDto);
        Task<MyResult> DeleteProductReviewAsync(int reviewId);
    }
}
