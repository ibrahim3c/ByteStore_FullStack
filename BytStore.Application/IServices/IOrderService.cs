using ByteStore.Domain.Abstractions.Enums;
using ByteStore.Domain.Abstractions.Result;
using BytStore.Application.DTOs.Order;
using BytStore.Application.DTOs.Product;

namespace BytStore.Application.IServices
{
    public interface IOrderService
    {
        Task<Result2> PlaceOrderAsync(PlaceOrderDto dto, string userId);
        Task<Result2<IEnumerable<OrderDto>>> GetCustomerOrdersAsync(Guid customerId);
        Task<Result2<OrderDto>> GetOrderByIdAsync(Guid orderId);

        // Admin
        Task<Result2<IEnumerable<OrderDto>>> GetAllOrdersAsync();
        Task<Result2<PagedDto<OrderDto>>> GetAllOrderstsAsync(int pageNumber, int pageSize);
        Task<Result2> UpdateOrderStatusAsync(Guid orderId, OrderStatus newStatus);
    }
}
