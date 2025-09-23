using ByteStore.Domain.Abstractions.Result;
using ByteStore.Domain.Entities;
using ByteStore.Domain.Repositories;
using BytStore.Application.DTOs.Customer;
using BytStore.Application.Services;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace ByteStore.Application.UnitTests.Services
{
    public class CustomerServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly Mock<IBaseRepository<Address>> _mockAddressRepository;
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _mockAddressRepository = new Mock<IBaseRepository<Address>>();

            // Setup the UnitOfWork to return our specific and generic mock repositories
            _mockUnitOfWork.Setup(uow => uow.CustomerRepository).Returns(_mockCustomerRepository.Object);
            _mockUnitOfWork.Setup(uow => uow.GetRepository<Address>()).Returns(_mockAddressRepository.Object);

            _customerService = new CustomerService(_mockUnitOfWork.Object);
        }

        #region Customer Operations

        [Fact]
        public async Task GetCustomerProfileByIdAsync_ShouldReturnSuccess_WhenCustomerAndUserExist()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customer = new Customer
            {
                Id = customerId,
                FirstName = "John",
                LastName = "Doe",
                AppUserId = "dkdkll43"
            };
            _mockCustomerRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Customer, bool>>>(), It.IsAny<string[]>()))
                                 .ReturnsAsync(customer);

            // Act
            var result = await _customerService.GetCustomerProfileByIdAsync(customerId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Email.Should().Be(customer.AppUser.Email);
            result.Value.FirstName.Should().Be(customer.FirstName);
        }

        [Fact]
        public async Task GetCustomerProfileByIdAsync_ShouldReturnFailure_WhenCustomerNotFound()
        {
            // Arrange
            _mockCustomerRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Customer, bool>>>(), It.IsAny<string[]>()))
                                 .ReturnsAsync((Customer)null);

            // Act
            var result = await _customerService.GetCustomerProfileByIdAsync(Guid.NewGuid());

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(CustomerErrors.NotFound);
        }

        [Fact]
        public async Task UpdateCustomerProfileAsync_ShouldReturnSuccess_WhenCustomerIsValid()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customer = new Customer
            {
                Id = customerId,
                FirstName = "Old",
                LastName = "Name",
                AppUserId = "dkdkll43"
            };
            var updateDto = new CustomerUpdateDto
            {
                FirstName = "New",
                LastName = "Name",
                PhoneNumber = "222"
            };

            _mockCustomerRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Customer, bool>>>(), It.IsAny<string[]>()))
                                 .ReturnsAsync(customer);

            // Act
            var result = await _customerService.UpdateCustomerProfileAsync(customerId, updateDto);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mockCustomerRepository.Verify(repo => repo.Update(It.Is<Customer>(c => c.FirstName == "New" && c.AppUser.PhoneNumber == "222")), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteCustomerAsync_ShouldSetIsDeletedToTrue_WhenCustomerExists()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customer = new Customer { Id = customerId, IsDeleted = false };
            _mockCustomerRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Customer, bool>>>(), null))
                                 .ReturnsAsync(customer);

            // Act
            var result = await _customerService.DeleteCustomerAsync(customerId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mockCustomerRepository.Verify(repo => repo.Update(It.Is<Customer>(c => c.IsDeleted == true)), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        #endregion

        #region Address Operations

        [Fact]
        public async Task GetCustomerAddressesAsync_ShouldReturnAddresses_WhenCustomerExists()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var customer = new Customer
            {
                Id = customerId,
                FirstName = "Jane",
                LastName = "Doe",
                Addresses = new List<Address>
                {
                    new Address { Id = 1, Street = "123 Main St", City = "Anytown" }
                }
            };
            _mockCustomerRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Customer, bool>>>(), It.IsAny<string[]>()))
                                 .ReturnsAsync(customer);

            // Act
            var result = await _customerService.GetCustomerAddressesAsync(customerId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().HaveCount(1);
            result.Value.First().Street.Should().Be("123 Main St");
        }

        [Fact]
        public async Task AddAddressAsync_ShouldReturnSuccess_WhenDataIsValid()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var addressDto = new AddressDto
            {
                Street = "456 Oak Ave",
                City = "Someplace",
                AddressType = "Shipping"
            };

            // Act
            var result = await _customerService.AddAddressAsync(customerId, addressDto);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mockAddressRepository.Verify(repo => repo.AddAsync(It.Is<Address>(a => a.Street == "456 Oak Ave" && a.CustomerId == customerId)), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAddressAsync_ShouldReturnFailure_WhenAddressNotFound()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var addressId = 1;
            var addressDto = new AddressDto();

            _mockAddressRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Address, bool>>>(), null))
                                  .ReturnsAsync((Address)null);

            // Act
            var result = await _customerService.UpdateAddressAsync(customerId, addressId, addressDto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(AddressErrors.NotFound);
        }

        [Fact]
        public async Task DeleteAddressAsync_ShouldReturnSuccess_WhenAddressExists()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var addressId = 1;
            var address = new Address { Id = addressId, CustomerId = customerId };

            _mockAddressRepository.Setup(repo => repo.FindAsync(It.IsAny<Expression<Func<Address, bool>>>(), null))
                                  .ReturnsAsync(address);

            // Act
            var result = await _customerService.DeleteAddressAsync(customerId, addressId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mockAddressRepository.Verify(repo => repo.Delete(address), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        #endregion
    }
}
