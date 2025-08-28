using ByteStore.Domain.Abstractions;
using ByteStore.Domain.Repositories;
using BytStore.Application.DTOs.Product;

namespace BytStore.Application.Services
{
    public class ProductService
    {
        private readonly IUnitOfWork unitOfWork;
        public ProductService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

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
        // add product 
        // update product
        // delete product

        // get product images
        // add product image
        // delete product image

    }
}
