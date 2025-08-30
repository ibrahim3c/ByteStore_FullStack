using ByteStore.Domain.Abstractions;
using ByteStore.Domain.Entities;
using ByteStore.Domain.Repositories;
using BytStore.Application.DTOs.Product;
using ByteStore.Domain.Abstractions;

namespace BytStore.Application.Services
{
    public class ProductService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ImageService imageService;
        public ProductService(IUnitOfWork unitOfWork, ImageService imageService)
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
        public async Task<ByteStore.Domain.Abstractions.Result> UpdateProductAsync(int productId, ProductUpdateDto productUpdateDto)
        {
            // TODO: validate dto
            var product = await unitOfWork.ProductRepository.GetByIdAsync(productId);
            if (product == null)
                return ByteStore.Domain.Abstractions.Result.Failure(new List<string> { "Product not found." });
            product.Name = productUpdateDto.Name;
            product.Description = productUpdateDto.Description;
            product.Price = productUpdateDto.Price;
            product.StockQuantity = productUpdateDto.StockQuantity;
            product.CategoryId = productUpdateDto.CategoryId;
            product.BrandId = productUpdateDto.BrandId;
            product.UpdatedAt = DateTime.UtcNow;

            unitOfWork.ProductRepository.Update(product);
            await unitOfWork.SaveChangesAsync();
            return ByteStore.Domain.Abstractions.Result.Success();
        }

        public async Task<ByteStore.Domain.Abstractions.Result> DeleteProductAsync(int productId)
        {
            var product = await unitOfWork.ProductRepository.GetByIdAsync(productId);
            if (product == null)
                return ByteStore.Domain.Abstractions.Result.Failure(new List<string> { "Product not found." });
            unitOfWork.ProductRepository.Delete(product);

            // delete all product images from imagekit

            await unitOfWork.SaveChangesAsync();
            return ByteStore.Domain.Abstractions.Result.Success();
        }


        // product image operations
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
        
        public async Task<ByteStore.Domain.Abstractions.Result> DeleteProductImageAsync(int productImageId)
        {
            var productImage = await unitOfWork.GetRepository<ProductImage>().GetByIdAsync(productImageId);
            if (productImage == null)
                return ByteStore.Domain.Abstractions.Result.Failure(new List<string> { "Product image not found." });
            var deleteResult = await imageService.DeleteAsync(productImage.FileId);
            if (!deleteResult.IsSuccess)
            {
                return ByteStore.Domain.Abstractions.Result.Failure(deleteResult.Errors);
            }
            unitOfWork.GetRepository<ProductImage>().Delete(productImage);
            await unitOfWork.SaveChangesAsync();
            return ByteStore.Domain.Abstractions.Result.Success();
        }
        public async Task<ByteStore.Domain.Abstractions.Result> SetPrimaryProductImageAsync( int productImageId)
        {
            var productImage = await unitOfWork.GetRepository<ProductImage>().GetByIdAsync(productImageId);
            if (productImage == null)
                return ByteStore.Domain.Abstractions.Result.Failure(new List<string> { "Product image not found." });

            var productImages = await unitOfWork.GetRepository<ProductImage>().FindAllAsync(pi => pi.ProductId == productImage.ProductId);
            foreach (var img in productImages)
            {
                //img.IsPrimary = img.Id == productImageId;
                //unitOfWork.GetRepository<ProductImage>().Update(img);
            }
            await unitOfWork.SaveChangesAsync();
            return ByteStore.Domain.Abstractions.Result.Success();
        }

        // product review operations
        // add review
        // update review
        // delete review
    }
}
