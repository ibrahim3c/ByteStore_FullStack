using ByteStore.Domain.Abstractions.Enums;
using ByteStore.Domain.Abstractions.Result;
using BytStore.Application.DTOs.Order;
using BytStore.Application.DTOs.Product;

namespace BytStore.Application.IServices
{
    public interface IOrderService
    {
        Task<Result> PlaceOrderAsync(PlaceOrderDto dto, string userId);
        Task<Result<IEnumerable<OrderDto>>> GetCustomerOrdersAsync(string userId);
        Task<Result<OrderDto>> GetOrderByIdAsync(Guid orderId, string userId);
        Task<Result> CancelOrderAsync(Guid orderId, string userId);

        // Admin
        Task<Result<IEnumerable<OrderDto>>> GetAllOrdersAsync();
        Task<Result<PagedDto<OrderDto>>> GetAllOrderstsAsync(int pageNumber, int pageSize);
        Task<OrderDto> GetOrderByIdAsync(Guid orderId);
        Task<Result<IEnumerable<OrderDto>>> SearchOrdersAsync(string query);
        Task<Result> UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus);
    }
}
