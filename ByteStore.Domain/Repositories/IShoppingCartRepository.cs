using ByteStore.Domain.Entities;

namespace ByteStore.Domain.Repositories
{
    public interface IShoppingCartRepository
    {
        Task<bool> ClearCartAsync(string customerId);
        Task<ShoppingCart?> GetCartAsync(string customerId);
        Task<bool> SaveCartAsync(ShoppingCart cart);
    }
}
