using ByteStore.Domain.Entities;
using ByteStore.Domain.Repositories;
using ByteStore.Persistance.Database;

namespace ByteStore.Persistance.Repositories
{
    internal class ShoppingCartRepository:BaseRepository<ShoppingCart>, IShoppingCartRepository
    {
        public ShoppingCartRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
