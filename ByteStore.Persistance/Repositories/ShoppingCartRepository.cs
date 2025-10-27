﻿using ByteStore.Domain.Entities;
using ByteStore.Domain.Repositories;
using StackExchange.Redis;
using System.Text.Json;

namespace ByteStore.Persistance.Repositories
{
    internal class ShoppingCartRepository: IShoppingCartRepository
    {
        private readonly IDatabase _db;

        public ShoppingCartRepository(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }
        public async Task<bool> ClearCartAsync(string id)
        {
            return await _db.KeyDeleteAsync(GetCartKey(id));
        }

        public async Task<ShoppingCart?> GetCartAsync(string id)
        {
            var cart = await _db.StringGetAsync(GetCartKey(id));
            return cart.IsNullOrEmpty? null:JsonSerializer.Deserialize<ShoppingCart>(cart);
        }

        public async Task<bool> SaveCartAsync(ShoppingCart cart)
        {
            var cartJson = JsonSerializer.Serialize(cart);
            return await _db.StringSetAsync(GetCartKey(cart.Id), cartJson, TimeSpan.FromDays(30));
        }
        private string GetCartKey(string customerId) => $"cart:{customerId}";

    }
}
