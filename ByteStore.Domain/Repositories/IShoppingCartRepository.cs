using ByteStore.Domain.Entities;

namespace ByteStore.Domain.Repositories
{
    public interface IShoppingCartRepository
    {
        Task<bool> ClearCartAsync(string id);
        Task<ShoppingCart?> GetCartAsync(string id);
        Task<bool> SaveCartAsync(ShoppingCart cart);
    }
}
