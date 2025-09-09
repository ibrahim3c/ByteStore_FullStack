using ByteStore.Domain.Abstractions.Result;
using ByteStore.Domain.Entities;

namespace BytStore.Application.IServices
{
    public interface IShoppingCartService
    {
        Task<Result2> ClearCartAsync(string customerId);
        Task<Result2<ShoppingCart>> GetCartAsync(string customerId);
        // for add or update
        Task<Result2> SaveCartAsync(ShoppingCart cart);
    }
}
