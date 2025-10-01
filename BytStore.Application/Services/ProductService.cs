using ByteStore.Domain.Abstractions.Result;
using ByteStore.Domain.Abstractions.Shared;
using ByteStore.Domain.Entities;
using ByteStore.Domain.Repositories;
using ByteStore.Domain.Specifications;
using BytStore.Application.DTOs.Product;
using BytStore.Application.IServices;
using BytStore.Application.Specifications;
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
        public async Task<Result2<IEnumerable<ProductListDto>>> GetAllProductsAsync()
        {
            var products = await unitOfWork.ProductRepository.GetAllAsync(["Category", "Brand", "Images"]);

            var productsDto= products.Select(p => new ProductListDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CategoryName = p.Category.Name,
                BrandName = p.Brand.Name,
                ThumbnailUrl = p.Images.FirstOrDefault(pi => pi.IsPrimary)?.ImageUrl
            });
            return Result2<IEnumerable<ProductListDto>>.Success(productsDto);
        }

        // just pagination
        //public async Task<Result2<PagedList<ProductListDto>>> GetAllProductsAsync(RequestParameters parameters)
        //{
        //    var products = await unitOfWork.ProductRepository.PaginateAsync(parameters.PageNumber, parameters.PageSize, null, ["Category", "Brand", "Images"]);
        //    var productsListDto = products.Select(p => new ProductListDto
        //    {
        //        Id = p.Id,
        //        Name = p.Name,
        //        Price = p.Price,
        //        CategoryName = p.Category.Name,
        //        BrandName = p.Brand.Name,
        //        ThumbnailUrl = p.Images.FirstOrDefault(pi => pi.IsPrimary)?.ImageUrl
        //    }).ToList();

        //    var totalCount = await unitOfWork.ProductRepository.CountAsync();

        //    var PagedList=new PagedList<ProductListDto>(productsListDto, totalCount,parameters.PageNumber,parameters.PageSize);
        //    return Result2<PagedList<ProductListDto>>.Success(PagedList);
        //}

        // pagination + filtering + searching + sorting
        public async Task<Result2<PagedList<ProductListDto>>> GetAllProductsAsync(ProductParameters parameters)
        {
            if (!parameters.ValidPriceRange)
                return Result2<PagedList<ProductListDto>>.Failure(ProductErrors.InvalidPriceRange);

            var spec = new ProductSpecification(parameters);
            var productsListDto = (await unitOfWork.ProductRepository.FindAllAsync(spec))
                .Select(p => new ProductListDto
                { 
                    Id = p.Id, Name = p.Name,
                    Price = p.Price,
                    CategoryName = p.Category.Name, 
                    BrandName = p.Brand.Name,
                    ThumbnailUrl = p.Images.FirstOrDefault(pi => pi.IsPrimary)?.ImageUrl }
                ).ToList(); 
            var totalCount = await unitOfWork.ProductRepository.CountAsync(spec);

            var PagedList = new PagedList<ProductListDto>(productsListDto, totalCount, parameters.PageNumber, parameters.PageSize);
            return Result2<PagedList<ProductListDto>>.Success(PagedList);
        }
        public async Task<Result2<PagedList<ProductListDto>>> GetAllProducts2Async(ProductParameters parameters)
        {
            if (!parameters.ValidPriceRange)
                return Result2<PagedList<ProductListDto>>.Failure(ProductErrors.InvalidPriceRange);

            var spec = new ProductWithoutPaginationSpecification(parameters);
            var productsListDto = (await unitOfWork.ProductRepository.FindAllAsync(spec))
                .Select(p => new ProductListDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    CategoryName = p.Category.Name,
                    BrandName = p.Brand.Name,
                    ThumbnailUrl = p.Images.FirstOrDefault(pi => pi.IsPrimary)?.ImageUrl
                }
                ).ToList();

            var PagedList = PagedList<ProductListDto>.ToPagedList(productsListDto, parameters.PageNumber, parameters.PageSize);
            return Result2<PagedList<ProductListDto>>.Success(PagedList);
        }
        public async Task<Result2<ProductDetailsDto>> GetProductByIdAsync(int productId)
        {
            var product = await unitOfWork.ProductRepository.FindAsync(p=>p.Id==productId, new string[] { "Category", "Brand", "Images", "Reviews.Customer" });
            if (product == null) return Result2<ProductDetailsDto>.Failure(ProductErrors.NotFound);
                
            var productDetailsDto=new ProductDetailsDto
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
            return Result2<ProductDetailsDto>.Success(productDetailsDto);
        }
        public async Task<Result2<IEnumerable<ProductListDto>>> GetProductsByCategoryIdAsync(int categoryId)
        {
            var products = await unitOfWork.ProductRepository.FindAllAsync(p=>p.CategoryId==categoryId,["Category", "Brand", "Images"]);

            var productsDto = products.Select(p => new ProductListDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CategoryName = p.Category.Name,
                BrandName = p.Brand.Name,
                ThumbnailUrl = p.Images.FirstOrDefault(pi => pi.IsPrimary)?.ImageUrl
            });
            return Result2<IEnumerable<ProductListDto>>.Success(productsDto);
        }
        public async Task<Result2<IEnumerable<ProductListDto>>> GetProductsByBrandIdAsync(int brandId)
        {
            var products = await unitOfWork.ProductRepository.FindAllAsync(p => p.BrandId == brandId, ["Category","Brand", "Images"]);
            var productsDto = products.Select(p => new ProductListDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CategoryName = p.Category.Name,
                BrandName = p.Brand.Name,
                ThumbnailUrl = p.Images.FirstOrDefault(pi => pi.IsPrimary)?.ImageUrl
            });
            return Result2<IEnumerable<ProductListDto>>.Success(productsDto);
        }
        // search products by name or description => list of ProductListDto
        public async Task<Result2<IEnumerable<ProductListDto>>> SearchProductsAsync(string query)
        {
            var products = await unitOfWork.ProductRepository.FindAllAsync(p => p.Name.Contains(query) || p.Description.Contains(query), ["Category", "Brand", "Images"]);
            var productsDto = products.Select(p => new ProductListDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                CategoryName = p.Category.Name,
                BrandName = p.Brand.Name,
                ThumbnailUrl = p.Images.FirstOrDefault(pi => pi.IsPrimary)?.ImageUrl
            });
            return Result2<IEnumerable<ProductListDto>>.Success(productsDto);
        }
        public async Task<Result2<int>> AddProductAsync(ProductCreateDto productCreateDto)
        {
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
            return Result2<int>.Success(product.Id);
        }
        public async Task<Result2> UpdateProductAsync(int productId, ProductUpdateDto productUpdateDto)
        {
            // TODO: validate dto
            var product = await unitOfWork.ProductRepository.GetByIdAsync(productId);
            if (product == null)
                return Result2.Failure(ProductErrors.NotFound);
            product.Name = productUpdateDto.Name;
            product.Description = productUpdateDto.Description;
            product.Price = productUpdateDto.Price;
            product.StockQuantity = productUpdateDto.StockQuantity;
            product.CategoryId = productUpdateDto.CategoryId;
            product.BrandId = productUpdateDto.BrandId;
            product.UpdatedAt = DateTime.UtcNow;

            unitOfWork.ProductRepository.Update(product);
            await unitOfWork.SaveChangesAsync();
            return Result2.Success();
        }
        public async Task<Result2> DeleteProductAsync(int productId)
        {
            var product = await unitOfWork.ProductRepository.GetByIdAsync(productId);
            if (product == null)
                return Result2.Failure(ProductErrors.NotFound);
            unitOfWork.ProductRepository.Delete(product);

            // delete related images from ImageKit and database
            var productImages = await unitOfWork.GetRepository<ProductImage>().FindAllAsync(pi => pi.ProductId == productId);
            foreach (var pi in productImages)
            {
                var deleteResult = await imageService.DeleteAsync(pi.FileId);
                if (!deleteResult.IsSuccess)
                {
                    return Result2.Failure(deleteResult.Error);
                }
                unitOfWork.GetRepository<ProductImage>().Delete(pi);
            }

            await unitOfWork.SaveChangesAsync();
            return Result2.Success();
        }


        // product image operations
        public async Task<Result2<IEnumerable<ProductImageDto>>> GetProductImagesAsync(int productId)
        {
            var product = await unitOfWork.ProductRepository.GetByIdAsync(productId);
            if (product == null)
                return Result2<IEnumerable<ProductImageDto>>.Failure(ProductErrors.NotFound);

            var productImages = await unitOfWork.GetRepository<ProductImage>().FindAllAsync(pi => pi.ProductId == productId);
            var productImagesDto = productImages.Select(pi => new ProductImageDto
            {
                Id = pi.id,
                ImageUrl = pi.ImageUrl,
                IsPrimary = pi.IsPrimary
            });
            return Result2<IEnumerable<ProductImageDto>>.Success(productImagesDto);
        }
        public async Task<Result2<int>> AddProductImagesAsync(int productId,List<ProductImageCreateDto> productImageCreateDtos)
        {
            var product = await unitOfWork.ProductRepository.GetByIdAsync(productId);
            if (product == null)
                return Result2<int>.Failure(ProductErrors.NotFound);

            var primaryCount = productImageCreateDtos.Count(p => p.IsPrimary);
            if (primaryCount > 1)
            {
                return Result2<int>.Failure(ProductErrors.MultiplePrimaryImage);
            }

            // check if list has primary image and this product has already image so return error
            var existingPrimary = await unitOfWork.GetRepository<ProductImage>()
                    .AnyAsync(pi => pi.ProductId == productId && pi.IsPrimary);

            if (existingPrimary && productImageCreateDtos.Any(p => p.IsPrimary))
            {
                return Result2<int>.Failure(ProductErrors.AlreadyHasPrimaryImage);
            }

            foreach (var picDto in productImageCreateDtos)
            {
                var uploadResult = await imageService.UploadAsync(picDto.Image, "products");
                if (!uploadResult.IsSuccess)
                {
                    return Result2<int>.Failure(uploadResult.Error);
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
            return Result2<int>.Success(productId);

        }
        public async Task<Result2> DeleteProductImageAsync(int productImageId)
        {
            var productImage = await unitOfWork.GetRepository<ProductImage>().GetByIdAsync(productImageId);
            if (productImage == null)
                return Result2.Failure(ProductErrors.ImageNotFound);
            var deleteResult = await imageService.DeleteAsync(productImage.FileId);
            if (!deleteResult.IsSuccess)
            {
                return Result2.Failure(deleteResult.Error);
            }

            if (productImage.IsPrimary)
            {
                var otherImage = await unitOfWork.GetRepository<ProductImage>().FindAsync(img => img.ProductId == productImage.ProductId && img.id != productImage.id);
                if (otherImage != null)
                {
                    otherImage.IsPrimary = true;
                    unitOfWork.GetRepository<ProductImage>().Update(otherImage);
                }
            }


            unitOfWork.GetRepository<ProductImage>().Delete(productImage);
            await unitOfWork.SaveChangesAsync();
            return Result2.Success();
        }
        public async Task<Result2> SetPrimaryProductImageAsync( int productImageId)
        {
            var productImage = await unitOfWork.GetRepository<ProductImage>().GetByIdAsync(productImageId);
            if (productImage == null)
                return Result2.Failure(ProductErrors.ImageNotFound);

            var productImages = await unitOfWork.GetRepository<ProductImage>().FindAllAsync(pi => pi.ProductId == productImage.ProductId);
            foreach (var img in productImages)
            {
                if(img.id == productImageId)
                    img.IsPrimary = true;
                else
                    img.IsPrimary = false;
            }
            await unitOfWork.SaveChangesAsync();
            return Result2.Success();
        }

        // product review operations


        public async Task<Result2<IEnumerable<ProductReviewDto>>> GetProductReviewsAsync(int productId)
        {
            var product = await unitOfWork.ProductRepository.GetByIdAsync(productId);
            if (product == null)
                return Result2<IEnumerable<ProductReviewDto>>.Failure(ProductErrors.NotFound);

            var productReviews = await unitOfWork.GetRepository<ProductReview>().FindAllAsync(pr => pr.ProductId == productId, ["Customer"]);
            var productReviewsDto = productReviews.Select(r => new ProductReviewDto
            {
                Rating = r.Rating,
                Commenet = r.Comment,
                CreatedOn = r.CreatedOn,
                CustomerName = r.Customer?.fullName??""
            }).ToList();
            return Result2<IEnumerable<ProductReviewDto>>.Success(productReviewsDto);
        }
        public async Task<Result2<int>> AddProductReviewAsync(int productId, ProductReviewCreateDto productReviewCreateDto)
        {
            var product = await unitOfWork.ProductRepository.GetByIdAsync(productId);
            if (product == null)
                return Result2<int>.Failure(ProductErrors.NotFound);

            var customer = await unitOfWork.CustomerRepository.FindAsync(c=>c.Id==productReviewCreateDto.CustomerId);
            if (customer == null)
                return Result2<int>.Failure(CustomerErrors.NotFound);

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
            return Result2<int>.Success(review.Id);
        }
        public async Task<Result2> UpdateProductReviewAsync(int reviewId, ProductReviewUpdateDto productReviewUpdateDto)
        {
            var review = await unitOfWork.GetRepository<ProductReview>().GetByIdAsync(reviewId);
            if (review == null)
                return Result2.Failure(ProductErrors.ReviewNotFound);
            review.Rating = productReviewUpdateDto.Rating;
            review.Comment = productReviewUpdateDto.Comment;
            await unitOfWork.SaveChangesAsync();
            return Result2.Success();
        }
        public async Task<Result2> DeleteProductReviewAsync(int reviewId)
        {
            var review = await unitOfWork.GetRepository<ProductReview>().GetByIdAsync(reviewId);
            if (review == null)
                return Result2.Failure(ProductErrors.ReviewNotFound);
            unitOfWork.GetRepository<ProductReview>().Delete(review);
            await unitOfWork.SaveChangesAsync();
            return Result2.Success();
        }
    }
}
