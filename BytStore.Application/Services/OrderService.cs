using ByteStore.Domain.Abstractions.Constants;
using ByteStore.Domain.Abstractions.Enums;
using ByteStore.Domain.Abstractions.Result;
using ByteStore.Domain.Entities;
using ByteStore.Domain.Repositories;
using BytStore.Application.DTOs.Order;
using BytStore.Application.DTOs.Product;
using BytStore.Application.IServices;

namespace BytStore.Application.Services
{
    internal class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;

        public OrderService(IUnitOfWork unitOfWork )
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result2<IEnumerable<OrderDto>>> GetAllOrdersAsync()
        {
            var orders = (await unitOfWork.OrderRepository.GetAllAsync(["Customer", "ShippingAddress", "BillingAddress", "OrderItems.Product"])).Select(o => new OrderDto
            {
                CustomerName = o.Customer.fullName,
                CustomerId = o.CustomerId,
                ShippingAddressId = o.ShippingAddressId,
                BillingAddressId = o.BillingAddressId,
                BillingAddress=new OrderAddressDto
                {
                    City=o.BillingAddress.City, 
                    Country=o.BillingAddress.Country,
                    IsPrimary=o.BillingAddress.IsPrimary,
                    PostalCode=o.BillingAddress.PostalCode,
                    State=o.BillingAddress.State,
                    Street=o.BillingAddress.Street,
                },
                ShippingAddress = new OrderAddressDto
                {
                    City = o.BillingAddress.City,
                    Country = o.BillingAddress.Country,
                    IsPrimary = o.BillingAddress.IsPrimary,
                    PostalCode = o.BillingAddress.PostalCode,
                    State = o.BillingAddress.State,
                    Street = o.BillingAddress.Street,
                },
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                OrderItems = o.OrderItems.Select(o => new GetOrderItemDto
                {
                    Id = o.Id,
                    ProductId = o.ProductId,
                    ProductName = o.Product.Name,
                    Quantity = o.Quantity,
                    UnitPrice = o.UnitPrice
                }).ToList()
            });


            return  Result2<IEnumerable<OrderDto>>.Success(orders);
        }
        public async Task<Result2<PagedDto<OrderDto>>> GetAllOrderstsAsync(int pageNumber, int pageSize)
        {
            // Validate input parameters
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100; // Prevent excessive page sizes
            var orders = (await unitOfWork.OrderRepository.GetAllAsync(["Customer", "ShippingAddress", "BillingAddress", "OrderItems.Product"])).Select(o => new OrderDto
            {
                CustomerName = o.Customer.fullName,
                CustomerId = o.CustomerId,
                ShippingAddressId = o.ShippingAddressId,
                BillingAddressId = o.BillingAddressId,
                BillingAddress = new OrderAddressDto
                {
                    City = o.BillingAddress.City,
                    Country = o.BillingAddress.Country,
                    IsPrimary = o.BillingAddress.IsPrimary,
                    PostalCode = o.BillingAddress.PostalCode,
                    State = o.BillingAddress.State,
                    Street = o.BillingAddress.Street,
                },
                ShippingAddress = new OrderAddressDto
                {
                    City = o.BillingAddress.City,
                    Country = o.BillingAddress.Country,
                    IsPrimary = o.BillingAddress.IsPrimary,
                    PostalCode = o.BillingAddress.PostalCode,
                    State = o.BillingAddress.State,
                    Street = o.BillingAddress.Street,
                },
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                OrderItems = o.OrderItems.Select(o => new GetOrderItemDto
                {
                    Id = o.Id,
                    ProductId = o.ProductId,
                    ProductName = o.Product.Name,
                    Quantity = o.Quantity,
                    UnitPrice = o.UnitPrice
                }).ToList()
            });

            var totalCount = await unitOfWork.ProductRepository.CountAsync();
            var pagedDto = new PagedDto<OrderDto>
            {
                Items = orders.ToList(),
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };

            return Result2<PagedDto<OrderDto>>.Success(pagedDto);



        }
        public async Task<Result2<IEnumerable<OrderDto>>> GetCustomerOrdersAsync(Guid customerId)
        {
            var orders = (await unitOfWork.OrderRepository.FindAllAsync(o=>o.CustomerId==customerId,["Customer", "ShippingAddress", "BillingAddress", "OrderItems.Product"])).Select(o => new OrderDto
            {
                CustomerName = o.Customer.fullName,
                CustomerId = o.CustomerId,
                ShippingAddressId = o.ShippingAddressId,
                BillingAddressId = o.BillingAddressId,
                BillingAddress = new OrderAddressDto
                {
                    City = o.BillingAddress.City,
                    Country = o.BillingAddress.Country,
                    IsPrimary = o.BillingAddress.IsPrimary,
                    PostalCode = o.BillingAddress.PostalCode,
                    State = o.BillingAddress.State,
                    Street = o.BillingAddress.Street,
                },
                ShippingAddress = new OrderAddressDto
                {
                    City = o.BillingAddress.City,
                    Country = o.BillingAddress.Country,
                    IsPrimary = o.BillingAddress.IsPrimary,
                    PostalCode = o.BillingAddress.PostalCode,
                    State = o.BillingAddress.State,
                    Street = o.BillingAddress.Street,
                },
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                OrderItems = o.OrderItems.Select(o => new GetOrderItemDto
                {
                    Id = o.Id,
                    ProductId = o.ProductId,
                    ProductName = o.Product.Name,
                    Quantity = o.Quantity,
                    UnitPrice = o.UnitPrice
                }).ToList()
            });


            return Result2<IEnumerable<OrderDto>>.Success(orders);
        }
        public async Task<Result2<OrderDto>> GetOrderByIdAsync(Guid orderId)
        {
            var o = await unitOfWork.OrderRepository.FindAsync(o => o.Id == orderId, ["Customer", "ShippingAddress", "BillingAddress", "OrderItems.Product"]);
            if (o is null)
                return Result2<OrderDto>.Failure(OrderErrors.NotFound);

            var orderDto= new OrderDto
            {
                CustomerName = o.Customer.fullName,
                CustomerId = o.CustomerId,
                ShippingAddressId = o.ShippingAddressId,
                BillingAddressId = o.BillingAddressId,
                BillingAddress = new OrderAddressDto
                {
                    City = o.BillingAddress.City,
                    Country = o.BillingAddress.Country,
                    IsPrimary = o.BillingAddress.IsPrimary,
                    PostalCode = o.BillingAddress.PostalCode,
                    State = o.BillingAddress.State,
                    Street = o.BillingAddress.Street,
                },
                ShippingAddress = new OrderAddressDto
                {
                    City = o.BillingAddress.City,
                    Country = o.BillingAddress.Country,
                    IsPrimary = o.BillingAddress.IsPrimary,
                    PostalCode = o.BillingAddress.PostalCode,
                    State = o.BillingAddress.State,
                    Street = o.BillingAddress.Street,
                },
                Id = o.Id,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                OrderItems = o.OrderItems.Select(o => new GetOrderItemDto
                {
                    Id = o.Id,
                    ProductId = o.ProductId,
                    ProductName = o.Product.Name,
                    Quantity = o.Quantity,
                    UnitPrice = o.UnitPrice
                }).ToList()
            };

            return Result2<OrderDto>.Success(orderDto);
        }

        public Task<Result2> PlaceOrderAsync(PlaceOrderDto dto, string userId)
        {
            throw new NotImplementedException();
        }
        public async Task<Result2> UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus)
        {
            var order = await unitOfWork.OrderRepository.FindAsync(o => o.Id == orderId);
            if (order is null)
                return Result2.Failure(OrderErrors.NotFound);
            order.Status = newStatus;
            return Result2.Success();
        }
    }
}
