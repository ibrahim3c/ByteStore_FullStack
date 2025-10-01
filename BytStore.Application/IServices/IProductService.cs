using ByteStore.Domain.Abstractions.Result;
using ByteStore.Domain.Abstractions.Shared;
using BytStore.Application.DTOs.Product;
using MyResult = ByteStore.Domain.Abstractions.Result.Result;

namespace BytStore.Application.IServices
{
    public interface IProductService
    {
        // Product operations
        Task<Result2<IEnumerable<ProductListDto>>> GetAllProductsAsync();
        //Task<Result2<PagedList<ProductListDto>>> GetAllProductsAsync(RequestParameters parameters);
        Task<Result2<PagedList<ProductListDto>>> GetAllProductsAsync(ProductParameters parameters);
        Task<Result2<PagedList<ProductListDto>>> GetAllProducts2Async(ProductParameters parameters);
        Task<Result2<ProductDetailsDto>> GetProductByIdAsync(int productId);
        Task<Result2<IEnumerable<ProductListDto>>> GetProductsByCategoryIdAsync(int categoryId);
        Task<Result2<IEnumerable<ProductListDto>>> GetProductsByBrandIdAsync(int brandId);
        Task<Result2<IEnumerable<ProductListDto>>> SearchProductsAsync(string query);
        Task<Result2<int>> AddProductAsync(ProductCreateDto productCreateDto);
        Task<Result2> UpdateProductAsync(int productId, ProductUpdateDto productUpdateDto);
        Task<Result2> DeleteProductAsync(int productId);

        // Product image operations
        Task<Result2<IEnumerable<ProductImageDto>>> GetProductImagesAsync(int productId);
        Task<Result2<int>> AddProductImagesAsync(int productId, List<ProductImageCreateDto> productImageCreateDtos);
        Task<Result2> DeleteProductImageAsync(int productImageId);
        Task<Result2> SetPrimaryProductImageAsync(int productImageId);

        // Product review operations
        Task<Result2<IEnumerable<ProductReviewDto>>> GetProductReviewsAsync(int productId);
        Task<Result2<int>> AddProductReviewAsync(int productId, ProductReviewCreateDto productReviewCreateDto);
        Task<Result2> UpdateProductReviewAsync(int reviewId, ProductReviewUpdateDto productReviewUpdateDto);
        Task<Result2> DeleteProductReviewAsync(int reviewId);
    }
}
