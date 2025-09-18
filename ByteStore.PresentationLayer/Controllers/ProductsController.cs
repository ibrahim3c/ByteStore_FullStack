using ByteStore.Domain.Abstractions.Constants;
using ByteStore.PresentationLayer.Controllers;
using BytStore.Application.DTOs.Product;
using BytStore.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ByteStore.Presentation.Controllers
{
    public class ProductsController : BaseController
    {
        public ProductsController(IServiceManager serviceManager) : base(serviceManager)
        {
            
        }
        // --- Product Operations ---

        // GET: api/products
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            // Use the paged version if pageNumber/pageSize are provided, otherwise get all
            if (pageNumber > 0 || pageSize > 0)
            {
                var pagedResult = await serviceManager.ProductService.GetAllProductsAsync(pageNumber, pageSize);
                return pagedResult.IsSuccess ? Ok(pagedResult.Value) : NotFound(pagedResult.Error);
            }
            else
            {
                var result = await serviceManager.ProductService.GetAllProductsAsync();
                return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
            }
        }

        // GET: api/products/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await serviceManager.ProductService.GetProductByIdAsync(id);
            return product != null ? Ok(product) : NotFound();
        }

        // GET: api/products/category/5
        [AllowAnonymous]
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            var result = await serviceManager.ProductService.GetProductsByCategoryIdAsync(categoryId);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        // GET: api/products/brand/3
        [AllowAnonymous]
        [HttpGet("brand/{brandId}")]
        public async Task<IActionResult> GetProductsByBrand(int brandId)
        {
            var result = await serviceManager.ProductService.GetProductsByBrandIdAsync(brandId);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        // GET: api/products/search?query=laptop
        [AllowAnonymous]
        [EnableRateLimiting("sliding")]
        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string query)
        {
            var result = await serviceManager.ProductService.SearchProductsAsync(query);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        // POST: api/products
        [HttpPost]
        [Authorize(Roles = Roles.AdminRole)]
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDto productCreateDto)
        {
            var result = await serviceManager.ProductService.AddProductAsync(productCreateDto);
            return result.IsSuccess
                ? CreatedAtAction(nameof(GetProductById), new { id = result.Value }, null)
                : BadRequest(result.Error);
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.AdminRole)]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDto productUpdateDto)
        {
            var result = await serviceManager.ProductService.UpdateProductAsync(id, productUpdateDto);
            return result.IsSuccess ? NoContent() : BadRequest(result.Error);
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.AdminRole)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var result = await serviceManager.ProductService.DeleteProductAsync(id);
            return result.IsSuccess ? NoContent() : NotFound(result.Error);
        }


        // --- Product Image Operations (Dependent Resource) ---

        // GET: api/products/5/images
        [HttpGet("{productId}/images")]
        [Authorize(Roles = Roles.AdminRole)]
        public async Task<IActionResult> GetProductImages(int productId)
        {
            var result = await serviceManager.ProductService.GetProductImagesAsync(productId);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        // POST: api/products/5/images
        [HttpPost("{productId}/images")]
        [Authorize(Roles = Roles.AdminRole)]
        public async Task<IActionResult> AddProductImages(int productId, [FromForm] List<ProductImageCreateDto> imageDtos)
        {
            var result = await serviceManager.ProductService.AddProductImagesAsync(productId, imageDtos);
            return result.IsSuccess ? StatusCode(201) : BadRequest(result.Error); // 201 Created
        }

        // PUT: api/products/images/15/set-primary
        [HttpPut("images/{imageId}/set-primary")]
        [Authorize(Roles = Roles.AdminRole)]
        public async Task<IActionResult> SetPrimaryImage(int imageId)
        {
            var result = await serviceManager.ProductService.SetPrimaryProductImageAsync(imageId);
            return result.IsSuccess ? NoContent() : BadRequest(result.Error);
        }

        // DELETE: api/products/images/15
        [HttpDelete("images/{imageId}")]
        [Authorize(Roles = Roles.AdminRole)]
        public async Task<IActionResult> DeleteProductImage(int imageId)
        {
            var result = await serviceManager.ProductService.DeleteProductImageAsync(imageId);
            return result.IsSuccess ? NoContent() : NotFound(result.Error);
        }


        // --- Product Review Operations (Dependent Resource) ---

        // POST: api/products/5/reviews
        [HttpPost("{productId}/reviews")]
        [Authorize(Roles = Roles.UserRole)]
        public async Task<IActionResult> AddProductReview(int productId, [FromBody] ProductReviewCreateDto reviewDto)
        {
            var result = await serviceManager.ProductService.AddProductReviewAsync(productId, reviewDto);
            return result.IsSuccess
                ? CreatedAtAction(nameof(GetProductById), new { id = productId }, null)
                : BadRequest(result.Error);
        }

        // PUT: api/products/reviews/8
        [HttpPut("reviews/{reviewId}")]
        [Authorize(Roles = Roles.UserRole)]
        public async Task<IActionResult> UpdateProductReview(int reviewId, [FromBody] ProductReviewUpdateDto reviewDto)
        {
            var result = await serviceManager.ProductService.UpdateProductReviewAsync(reviewId, reviewDto);
            return result.IsSuccess ? NoContent() : BadRequest(result.Error);
        }

        // DELETE: api/products/reviews/8
        [HttpDelete("reviews/{reviewId}")]
        [Authorize(Roles = Roles.UserRole)]
        public async Task<IActionResult> DeleteProductReview(int reviewId)
        {
            var result = await serviceManager.ProductService.DeleteProductReviewAsync(reviewId);
            return result.IsSuccess ? NoContent() : NotFound(result.Error);
        }

    }
}
