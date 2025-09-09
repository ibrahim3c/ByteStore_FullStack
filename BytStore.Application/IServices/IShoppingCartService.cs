using ByteStore.Domain.Abstractions.Result;
using BytStore.Application.DTOs.ShoppingCart;

namespace BytStore.Application.IServices
{
    public interface IShoppingCartService
    {
        Task<Result2> ClearCartAsync(string customerId);
        Task<Result2<ShoppingCartDto>> GetCartAsync(string customerId);
        // for add or update
        Task<Result2> SaveCartAsync(ShoppingCartDto cart);
    }
}
