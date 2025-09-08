using ByteStore.Domain.Entities;
using ByteStore.Domain.Repositories;
using StackExchange.Redis;

namespace ByteStore.Persistance.Repositories
{
    internal class ShoppingCartRepository: IShoppingCartRepository
    {
        private readonly IDatabase _db;

        public ShoppingCartRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            this._db = connectionMultiplexer.GetDatabase();
        }

        public Task DeleteShoppingCartAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ShoppingCart> GetShoppingCartAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateShippingCartAsync(int id, ShoppingCart shippingCart)
        {
            throw new NotImplementedException();
        }
    }
}
