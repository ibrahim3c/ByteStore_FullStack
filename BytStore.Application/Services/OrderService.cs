using ByteStore.Domain.Abstractions.Enums;
using ByteStore.Domain.Abstractions.Result;
using BytStore.Application.DTOs.Order;
using BytStore.Application.DTOs.Product;
using BytStore.Application.IServices;

namespace BytStore.Application.Services
{
    internal class OrderService : IOrderService
    {
        public Task<Result> CancelOrderAsync(Guid orderId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<OrderDto>>> GetAllOrdersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Result<PagedDto<OrderDto>>> GetAllOrderstsAsync(int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<OrderDto>>> GetCustomerOrdersAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<OrderDto>> GetOrderByIdAsync(Guid orderId, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<OrderDto> GetOrderByIdAsync(Guid orderId)
        {
            throw new NotImplementedException();
        }

        public Task<Result> PlaceOrderAsync(PlaceOrderDto dto, string userId)
        {
            throw new NotImplementedException();
        }

        public Task<Result<IEnumerable<OrderDto>>> SearchOrdersAsync(string query)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus)
        {
            throw new NotImplementedException();
        }
    }
}
