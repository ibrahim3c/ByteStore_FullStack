using ByteStore.Domain.Entities;
using ByteStore.Domain.Repositories;
using StackExchange.Redis;
using System.Text.Json;

namespace ByteStore.Persistance.Repositories
{
    internal class ShoppingCartRepository: IShoppingCartRepository
    {
        private readonly IDatabase _db;

        public async Task<bool> ClearCartAsync(string customerId)
        {
            return await _db.KeyDeleteAsync(GetCartKey(customerId));
        }

        public async Task<ShoppingCart?> GetCartAsync(string customerId)
        {
            var cart = await _db.StringGetAsync(customerId);
            return cart.IsNullOrEmpty? null:JsonSerializer.Deserialize<ShoppingCart>(cart);
        }

        public async Task<bool> SaveCartAsync(ShoppingCart cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            return await _db.StringSetAsync(GetCartKey(cart.CustomerId), cartJson, TimeSpan.FromDays(30));
        }
        private string GetCartKey(string customerId) => $"cart:{customerId}";

    }
}
