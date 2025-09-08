using ByteStore.Domain.Entities;

namespace ByteStore.Domain.Repositories
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCart> GetShoppingCartAsync(int id);
        Task DeleteShoppingCartAsync(int id);
        Task UpdateShippingCartAsync(int id,ShoppingCart shippingCart);
    }
}
