using ByteStore.Domain.Abstractions.Enums;
using ByteStore.Domain.Abstractions.Result;
using ByteStore.Domain.Entities;
using ByteStore.Domain.Repositories;
using BytStore.Application.DTOs.Order;
using BytStore.Application.DTOs.Product;
using BytStore.Application.IServices;
using Microsoft.EntityFrameworkCore;

namespace BytStore.Application.Services
{
    internal class OrderService : IOrderService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly IPaymentService paymentService;

        public OrderService(IUnitOfWork unitOfWork,IShoppingCartRepository shoppingCartRepository ,IPaymentService paymentService)
        {
            this.unitOfWork = unitOfWork;
            this.shoppingCartRepository = shoppingCartRepository;
            this.paymentService = paymentService;
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
        public async Task<Result2> PlaceOrderAsync(PlaceOrderDto dto)
        {
            var cart = await shoppingCartRepository.GetCartAsync(dto.CustomerId);
            if (cart is null)
                return Result2.Failure(CartErrors.NotFound);

            if (!Guid.TryParse(dto.CustomerId, out var customerId))
                return Result2.Failure(OrderErrors.InvalidCustomerId);

            var billingAddress = await unitOfWork.GetRepository<Address>().AnyAsync(a=>a.CustomerId==customerId && a.AddressType==AddressType.Billing);
            if (billingAddress)
                return Result2.Failure(OrderErrors.BillingAddressNotFound);

            var shippingAddress = await unitOfWork.GetRepository<Address>().AnyAsync(a => a.CustomerId == customerId && a.AddressType == AddressType.Shipping);
            if (shippingAddress)
                return Result2.Failure(OrderErrors.ShippingAddressNotFound);


            /*
            The Scenario 🎬
                A user wants to buy products.
                When they click Checkout, your backend creates a PaymentIntent in Stripe (this is like a transaction ID).
                You also need to create an Order in your database linked to that same PaymentIntentId.
            The Problem 🚨
                The user might accidentally checkout twice (e.g., double-click button, close browser, come back and retry).
                In that case:
                    Stripe will return the same PaymentIntentId (not a new one).
                    But if you just insert an order every time → you’ll end up with duplicate orders for the same payment.
            What the Code Does ✅
                Before creating a new order, it checks:“Do we already have an Order in the DB with this PaymentIntentId?”
                If it finds an existing order →
                    Deletes that old order (to avoid duplicates).
                    Updates the Stripe PaymentIntent (in case prices, delivery method, etc. changed).
                If no order exists → Just creates the new order using the PaymentIntentId.
             */

            var existingOrder = await unitOfWork.OrderRepository.FindAsync(
                m => m.PaymentIntentId == cart.PaymentIntentId);
            string paymentIntentId;
            if (existingOrder is not null)
            {
                // delete old order
                unitOfWork.OrderRepository.Delete(existingOrder);
                var result = await paymentService.CreateOrUpdatePaymentIntentAsync(cart.CustomerId);
                if (!result.IsSuccess)
                    return Result2.Failure(result.Error);
                paymentIntentId = result.Value.PaymentIntentId;
            }
            else
            {
                // reuse basket payment intent
                paymentIntentId = cart.PaymentIntentId;
            }
            // now create the order
            var order = new Order
            {
                BillingAddressId = dto.BillingAddressId,
                CustomerId = customerId,
                ShippingAddressId = dto.ShippingAddressId,
                PaymentIntentId = paymentIntentId
            };

            decimal totalAmount = 0;
            foreach (var item in cart.CartItems)
            {
                var product=await unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
                if(product == null)
                    return Result2.Failure(OrderErrors.ItemNotFound);

                if(product.StockQuantity <item.Quantity)
                    return Result2.Failure(OrderErrors.InsufficientStock);

                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = product.Price,
                    OrderId=order.Id
                };

                totalAmount += item.TotalPrice;
                order.OrderItems.Add(orderItem);

            // Update product stock
            product.StockQuantity -= item.Quantity;
            unitOfWork.ProductRepository.Update(product);
            }

            order.TotalAmount = totalAmount;

            // Save order
            await unitOfWork.OrderRepository.AddAsync(order);
            await unitOfWork.SaveChangesAsync();

            // Clear shopping cart
            await shoppingCartRepository.ClearCartAsync(dto.CustomerId);

            return Result2.Success();
        }
        public async Task<Result2> UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus)
        {
            // check if newStatus is valid in OrderStatus enum
            if (!Enum.IsDefined(typeof(OrderStatus), newStatus))
            {
                return Result2.Failure(OrderErrors.InvalidStatus);
            };
            var order = await unitOfWork.OrderRepository.FindAsync(o => o.Id == orderId);
            if (order is null)
                return Result2.Failure(OrderErrors.NotFound);
            order.Status = newStatus;
            return Result2.Success();
        }
    }
}
