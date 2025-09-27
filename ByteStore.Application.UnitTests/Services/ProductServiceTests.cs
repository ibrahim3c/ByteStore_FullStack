using ByteStore.Domain.Abstractions.Result;
using ByteStore.Domain.Entities;
using ByteStore.Domain.Repositories;
using BytStore.Application.DTOs.Product;
using BytStore.Application.DTOs.Shared;
using BytStore.Application.IServices;
using BytStore.Application.Services;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace ByteStore.Application.UnitTests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly Mock<IBaseRepository<ProductImage>> _mockProductImageRepository;
        private readonly Mock<IBaseRepository<ProductReview>> _mockProductReviewRepository;
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly Mock<IImageService> _mockImageService;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockProductRepository = new Mock<IProductRepository>();
            _mockProductImageRepository = new Mock<IBaseRepository<ProductImage>>();
            _mockProductReviewRepository = new Mock<IBaseRepository<ProductReview>>();
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _mockImageService = new Mock<IImageService>();

            _mockUnitOfWork.Setup(uow => uow.ProductRepository).Returns(_mockProductRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.CustomerRepository).Returns(_mockCustomerRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.GetRepository<ProductImage>()).Returns(_mockProductImageRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.GetRepository<ProductReview>()).Returns(_mockProductReviewRepository.Object);

            _productService = new ProductService(_mockUnitOfWork.Object, _mockImageService.Object);
        }

        #region Product Operations

        [Fact]
        public async Task GetAllProductsAsync_Paginated_ShouldReturnPagedList()
        {
            // Arrange
            var parameters = new RequestParameters { PageNumber = 1, PageSize = 10 };
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Laptop", Category = new Category(), Brand = new Brand(), Images = new List<ProductImage>() }
            };
            var totalCount = 25;

            _mockProductRepository.Setup(repo => repo.PaginateAsync(parameters.PageNumber, parameters.PageSize, null, It.IsAny<string[]>()))
                                .ReturnsAsync(products);
            _mockProductRepository
                .Setup(r => r.CountAsync())
                .ReturnsAsync(totalCount);

            // Act
            var result = await _productService.GetAllProductsAsync(parameters);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().HaveCount(1);
            result.Value.MetaData.TotalCount.Should().Be(totalCount);
        }

        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnSuccess_WhenProductExists()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Test",
                Description = "Desc",
                Price = 100,
                StockQuantity = 10,
                Category = new Category { Name = "Cat1" },
                Brand = new Brand { Name = "Brand1" },
                Images = new List<ProductImage>(),
                Reviews = new List<ProductReview>()
            };
            _mockProductRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Product, bool>>>(), It.IsAny<string[]>()))
                                .ReturnsAsync(product);

            // Act
            var result = await _productService.GetProductByIdAsync(1);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Name.Should().Be("Test");
        }

        [Fact]
        public async Task AddProductAsync_ShouldReturnSuccessWithProductId()
        {
            // Arrange
            var createDto = new ProductCreateDto { Name = "New Laptop" };
            _mockProductRepository.Setup(repo => repo.AddAsync(It.IsAny<Product>()))
                                .Callback<Product>(p => p.Id = 1); // Simulate DB assigning ID

            // Act
            var result = await _productService.AddProductAsync(createDto);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(1);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldReturnFailure_WhenProductNotFound()
        {
            // Arrange
            _mockProductRepository.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Product)null);

            // Act
            var result = await _productService.UpdateProductAsync(99, new ProductUpdateDto());

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(ProductErrors.NotFound);
        }

        [Fact]
        public async Task DeleteProductAsync_ShouldReturnSuccess_WhenProductAndImagesAreDeleted()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId };
            var images = new List<ProductImage> { new ProductImage { ProductId = productId, FileId = "file1" } };

            _mockProductRepository.Setup(repo => repo.GetByIdAsync(productId)).ReturnsAsync(product);
            _mockProductImageRepository.Setup(repo => repo.FindAllAsync(It.IsAny<Expression<Func<ProductImage, bool>>>(), null))
                                       .ReturnsAsync(images);
            _mockImageService.Setup(s => s.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                             .Returns(Task.FromResult(Result2.Success()));

            // Act
            var result = await _productService.DeleteProductAsync(productId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mockProductRepository.Verify(repo => repo.Delete(product), Times.Once);
            _mockImageService.Verify(s => s.DeleteAsync("file1", It.IsAny<CancellationToken>()), Times.Once);
            _mockProductImageRepository.Verify(repo => repo.Delete(It.IsAny<ProductImage>()), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        #endregion

        #region Product Image Operations

        [Fact]
        public async Task AddProductImagesAsync_ShouldReturnFailure_WhenMultipleImagesArePrimary()
        {
            // Arrange
            var product = new Product { Id = 1 };
            var imageDtos = new List<ProductImageCreateDto>
            {
                new ProductImageCreateDto { IsPrimary = true },
                new ProductImageCreateDto { IsPrimary = true }
            };
            _mockProductRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(product);

            // Act
            var result = await _productService.AddProductImagesAsync(1, imageDtos);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(ProductErrors.MultiplePrimaryImage);
        }

        [Fact]
        public async Task DeleteProductImageAsync_ShouldReturnSuccessAndReassignPrimary_WhenPrimaryIsDeleted()
        {
            // Arrange
            var primaryImage = new ProductImage { id = 1, ProductId = 1, FileId = "file1", IsPrimary = true };
            var otherImage = new ProductImage { id = 2, ProductId = 1, FileId = "file2", IsPrimary = false };
            var allImages = new List<ProductImage> { primaryImage, otherImage };

            _mockProductImageRepository.Setup(repo =>
                repo.GetByIdAsync(1))
                .ReturnsAsync(primaryImage);

            _mockImageService.Setup(s => s.DeleteAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Result2.Success());

            _mockProductImageRepository.Setup(repo =>
                   repo.FindAsync(It.IsAny<Expression<Func<ProductImage, bool>>>(), null))
                   .ReturnsAsync(otherImage);
            // Act
            var result = await _productService.DeleteProductImageAsync(1);

            // Assert
            result.IsSuccess.Should().BeTrue();
            otherImage.IsPrimary.Should().BeTrue(); // Verify primary was reassigned
            _mockProductImageRepository.Verify(repo => repo.Update(otherImage), Times.Once);
            _mockProductImageRepository.Verify(repo => repo.Delete(primaryImage), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        #endregion

        #region Product Review Operations

        [Fact]
        public async Task AddProductReviewAsync_ShouldReturnFailure_WhenCustomerNotFound()
        {
            // Arrange
            var product = new Product { Id = 1 };
            var reviewDto = new ProductReviewCreateDto { CustomerId = System.Guid.NewGuid() };

            _mockProductRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(product);
            _mockCustomerRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Customer, bool>>>(), null))
                                   .ReturnsAsync((Customer)null); // Customer not found

            // Act
            var result = await _productService.AddProductReviewAsync(1, reviewDto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(CustomerErrors.NotFound);
        }

        [Fact]
        public async Task DeleteProductReviewAsync_ShouldReturnSuccess_WhenReviewExists()
        {
            // Arrange
            var review = new ProductReview { Id = 1 };
            _mockProductReviewRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(review);

            // Act
            var result = await _productService.DeleteProductReviewAsync(1);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mockProductReviewRepository.Verify(repo => repo.Delete(review), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        #endregion
    }
}
