using ByteStore.Domain.Abstractions;
using ByteStore.Domain.Entities;
using ByteStore.Domain.Repositories;
using BytStore.Application.DTOs.Product;
using BytStore.Application.IServices;
using MyResult = ByteStore.Domain.Abstractions.Result;
namespace BytStore.Application.Services
{
    public class ProductService:IProductService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IImageService imageService;
        public ProductService(IUnitOfWork unitOfWork, IImageService imageService)
        {
            this.unitOfWork = unitOfWork;
            this.imageService = imageService;
        }

        // product operations
        public async Task<Result<IEnumerable<ProductListDto>>> GetAllProductsAsync()
        {
            var products = await unitOfWork.ProductRepository.GetAllAsync(["Category", "", "Brand", "Images"]);

            var productsDto= products.Select(p => new ProductListDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CategoryName = p.Category.Name,
                BrandName = p.Brand.Name,
                ThumbnailUrl = p.Images.FirstOrDefault(pi => pi.IsPrimary)?.ImageUrl
            });
            return Result<IEnumerable<ProductListDto>>.Success(productsDto);
        }
        public async Task<Result<PagedDto<ProductListDto>>> GetAllProductsAsync(int pageNumber, int pageSize)
        {
            // Validate input parameters
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100; // Prevent excessive page sizes

            var products = await unitOfWork.ProductRepository.PaginateAsync(pageNumber, pageSize, null, ["Category","", "Brand", "Images"]);
            var productsListDto= products.Select(p => new ProductListDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CategoryName = p.Category.Name,
                BrandName = p.Brand.Name,
                ThumbnailUrl= p.Images.FirstOrDefault(pi => pi.IsPrimary)?.ImageUrl
            });

            var totalCount = await unitOfWork.ProductRepository.CountAsync();
            var pagedDto=new PagedDto<ProductListDto>
            {
                Items = productsListDto.ToList(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };
            
            return Result<PagedDto<ProductListDto>>.Success(pagedDto);
        }
        public async Task<ProductDetailsDto> GetProductByIdAsync(int productId)
        {
            var product = await unitOfWork.ProductRepository.FindAsync(p=>p.Id==productId, new string[] { "Category", "Brand", "Images", "Reviews.Customer" });
            if (product == null) return null;

            return new ProductDetailsDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryName = product.Category.Name,
                BrandName = product.Brand.Name,
                Images = product.Images.Select(pi =>new ProductImageDto
                {
                    IsPrimary = pi.IsPrimary,
                    ImageUrl = pi.ImageUrl
                }).ToList(),
                Reviews = product.Reviews.Select(r => new ProductReviewDto
                {
                    Rating = r.Rating,
                    Commenet = r.Comment,
                    CreatedOn = r.CreatedOn,
                    CustomerName = r.Customer.fullName
                }).ToList()
            };
        }
        public async Task<Result<IEnumerable<ProductListDto>>> GetProductsByCategoryIdAsync(int categoryId)
        {
            var products = await unitOfWork.ProductRepository.FindAllAsync(p=>p.CategoryId==categoryId,["Category", "", "Brand", "Images"]);

            var productsDto = products.Select(p => new ProductListDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CategoryName = p.Category.Name,
                BrandName = p.Brand.Name,
                ThumbnailUrl = p.Images.FirstOrDefault(pi => pi.IsPrimary)?.ImageUrl
            });
            return Result<IEnumerable<ProductListDto>>.Success(productsDto);
        }
        public async Task<Result<IEnumerable<ProductListDto>>> GetProductsByBrandIdAsync(int brandId)
        {
            var products = await unitOfWork.ProductRepository.FindAllAsync(p => p.BrandId == brandId, ["Category", "", "Brand", "Images"]);
            var productsDto = products.Select(p => new ProductListDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CategoryName = p.Category.Name,
                BrandName = p.Brand.Name,
                ThumbnailUrl = p.Images.FirstOrDefault(pi => pi.IsPrimary)?.ImageUrl
            });
            return Result<IEnumerable<ProductListDto>>.Success(productsDto);
        }
        // search products by name or description => list of ProductListDto
        public async Task<Result<IEnumerable<ProductListDto>>> SearchProductsAsync(string query)
        {
            var products = await unitOfWork.ProductRepository.FindAllAsync(p => p.Name.Contains(query) || p.Description.Contains(query), ["Category", "", "Brand", "Images"]);
            var productsDto = products.Select(p => new ProductListDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CategoryName = p.Category.Name,
                BrandName = p.Brand.Name,
                ThumbnailUrl = p.Images.FirstOrDefault(pi => pi.IsPrimary)?.ImageUrl
            });
            return Result<IEnumerable<ProductListDto>>.Success(productsDto);
        }
        public async Task<Result<int>> AddProductAsync(ProductCreateDto productCreateDto)
        {
            // TODO: validate dto
            var product = new Product
            {
                Name = productCreateDto.Name,
                Description = productCreateDto.Description,
                Price = productCreateDto.Price,
                StockQuantity = productCreateDto.StockQuantity,
                CategoryId = productCreateDto.CategoryId,
                BrandId = productCreateDto.BrandId,
                CreatedAt = DateTime.UtcNow
            };
            await unitOfWork.ProductRepository.AddAsync(product);
            await unitOfWork.SaveChangesAsync();
            return Result<int>.Success(product.Id);
        }
        public async Task<MyResult> UpdateProductAsync(int productId, ProductUpdateDto productUpdateDto)
        {
            // TODO: validate dto
            var product = await unitOfWork.ProductRepository.GetByIdAsync(productId);
            if (product == null)
                return MyResult.Failure(new List<string> { "Product not found." });
            product.Name = productUpdateDto.Name;
            product.Description = productUpdateDto.Description;
            product.Price = productUpdateDto.Price;
            product.StockQuantity = productUpdateDto.StockQuantity;
            product.CategoryId = productUpdateDto.CategoryId;
            product.BrandId = productUpdateDto.BrandId;
            product.UpdatedAt = DateTime.UtcNow;

            unitOfWork.ProductRepository.Update(product);
            await unitOfWork.SaveChangesAsync();
            return MyResult.Success();
        }
        public async Task<MyResult> DeleteProductAsync(int productId)
        {
            var product = await unitOfWork.ProductRepository.GetByIdAsync(productId);
            if (product == null)
                return MyResult.Failure(new List<string> { "Product not found." });
            unitOfWork.ProductRepository.Delete(product);

            // delete related images from ImageKit and database
            var productImages = await unitOfWork.GetRepository<ProductImage>().FindAllAsync(pi => pi.ProductId == productId);
            foreach (var pi in productImages)
            {
                var deleteResult = await imageService.DeleteAsync(pi.FileId);
                if (!deleteResult.IsSuccess)
                {
                    return MyResult.Failure(deleteResult.Errors);
                }
                unitOfWork.GetRepository<ProductImage>().Delete(pi);
            }

            await unitOfWork.SaveChangesAsync();
            return MyResult.Success();
        }


        // product image operations
        public async Task<Result<IEnumerable<ProductImageDto>>> GetProductImagesAsync(int productId)
        {
            var product = await unitOfWork.ProductRepository.GetByIdAsync(productId);
            if (product == null)
                return Result<IEnumerable<ProductImageDto>>.Failure(new List<string> { "Product not found." });

            var productImages = await unitOfWork.GetRepository<ProductImage>().FindAllAsync(pi => pi.ProductId == productId);
            var productImagesDto = productImages.Select(pi => new ProductImageDto
            {
                Id = pi.id,
                ImageUrl = pi.ImageUrl,
                IsPrimary = pi.IsPrimary
            });
            return Result<IEnumerable<ProductImageDto>>.Success(productImagesDto);
        }
        public async Task<Result<int>> AddProductImagesAsync(int productId,List<ProductImageCreateDto> productImageCreateDtos)
        {
            var product = await unitOfWork.ProductRepository.GetByIdAsync(productId);
            if (product == null)
                return Result<int>.Failure(new List<string> { "Product not found." });

            foreach (var picDto in productImageCreateDtos)
            {
                var uploadResult = await imageService.UploadAsync(picDto.Image, "products");
                if (!uploadResult.IsSuccess)
                {
                    return Result<int>.Failure(uploadResult.Errors);
                }
                var productImage = new ProductImage
                {
                    ProductId = productId,
                    ImageUrl = uploadResult.Value.Url,
                    FileId = uploadResult.Value.FileId,
                    IsPrimary = picDto.IsPrimary
                };
                await unitOfWork.GetRepository<ProductImage>().AddAsync(productImage);
            }
            await unitOfWork.SaveChangesAsync();
            return Result<int>.Success(productId);

        }
        public async Task<MyResult> DeleteProductImageAsync(int productImageId)
        {
            var productImage = await unitOfWork.GetRepository<ProductImage>().GetByIdAsync(productImageId);
            if (productImage == null)
                return MyResult.Failure(new List<string> { "Product image not found." });
            var deleteResult = await imageService.DeleteAsync(productImage.FileId);
            if (!deleteResult.IsSuccess)
            {
                return MyResult.Failure(deleteResult.Errors);
            }

            if (productImage.IsPrimary)
            {
                var otherImage = (await unitOfWork.GetRepository<ProductImage>().GetAllAsync())
                    .Where(img => img.ProductId == productImage.ProductId && img.id != productImage.id)
                    .FirstOrDefault();
                if (otherImage != null)
                {
                    otherImage.IsPrimary = true;
                    unitOfWork.GetRepository<ProductImage>().Update(otherImage);
                }
            }


            unitOfWork.GetRepository<ProductImage>().Delete(productImage);
            await unitOfWork.SaveChangesAsync();
            return MyResult.Success();
        }
        public async Task<MyResult> SetPrimaryProductImageAsync( int productImageId)
        {
            var productImage = await unitOfWork.GetRepository<ProductImage>().GetByIdAsync(productImageId);
            if (productImage == null)
                return MyResult.Failure(new List<string> { "Product image not found." });

            var productImages = await unitOfWork.GetRepository<ProductImage>().FindAllAsync(pi => pi.ProductId == productImage.ProductId);
            foreach (var img in productImages)
            {
                if(img.id == productImageId)
                    img.IsPrimary = true;
                else
                    img.IsPrimary = false;
            }
            await unitOfWork.SaveChangesAsync();
            return MyResult.Success();
        }

        // product review operations
        public async Task<Result<int>> AddProductReviewAsync(int productId, ProductReviewCreateDto productReviewCreateDto)
        {
            var product = await unitOfWork.ProductRepository.GetByIdAsync(productId);
            if (product == null)
                return Result<int>.Failure(new List<string> { "Product not found." });
            var review = new ProductReview
            {
                ProductId = productId,
                CustomerId = productReviewCreateDto.CustomerId,
                Rating = productReviewCreateDto.Rating,
                Comment = productReviewCreateDto.Comment,
                CreatedOn = DateTime.UtcNow
            };
            await unitOfWork.GetRepository<ProductReview>().AddAsync(review);
            await unitOfWork.SaveChangesAsync();
            return Result<int>.Success(review.Id);
        }
        public async Task<MyResult> UpdateProductReviewAsync(int reviewId, ProductReviewUpdateDto productReviewUpdateDto)
        {
            var review = await unitOfWork.GetRepository<ProductReview>().GetByIdAsync(reviewId);
            if (review == null)
                return MyResult.Failure(new List<string> { "Review not found." });
            review.Rating = productReviewUpdateDto.Rating;
            review.Comment = productReviewUpdateDto.Comment;
            await unitOfWork.SaveChangesAsync();
            return MyResult.Success();
        }
        public async Task<MyResult> DeleteProductReviewAsync(int reviewId)
        {
            var review = await unitOfWork.GetRepository<ProductReview>().GetByIdAsync(reviewId);
            if (review == null)
                return MyResult.Failure(new List<string> { "Review not found." });
            unitOfWork.GetRepository<ProductReview>().Delete(review);
            await unitOfWork.SaveChangesAsync();
            return MyResult.Success();
        }
    }
}
