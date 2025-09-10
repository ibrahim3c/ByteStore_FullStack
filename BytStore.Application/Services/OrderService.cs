using ByteStore.Domain.Abstractions.Enums;
using ByteStore.Domain.Abstractions.Result;
using BytStore.Application.DTOs.Order;
using BytStore.Application.DTOs.Product;
using BytStore.Application.IServices;

namespace BytStore.Application.Services
{
    internal class OrderService : IOrderService
    {
        public Task<Result2> CancelOrderAsync(Guid orderId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<Result2<IEnumerable<OrderDto>>> GetAllOrdersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Result2<PagedDto<OrderDto>>> GetAllOrderstsAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<Result2<IEnumerable<OrderDto>>> GetCustomerOrdersAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<Result2<OrderDto>> GetOrderByIdAsync(Guid orderId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<Result2> GetOrderByIdAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<Result2> PlaceOrderAsync(PlaceOrderDto dto, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<Result2<IEnumerable<OrderDto>>> SearchOrdersAsync(string query)
        {
            throw new NotImplementedException();
        }

        public Task<Result2> UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus)
        {
            throw new NotImplementedException();
        }
    }
}
