using ByteStore.Domain.Entities;
using ByteStore.Domain.Repositories;
using ByteStore.Persistance.Database;



namespace ByteStore.Persistance.Repositories
{
    internal class ProductRepository:BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
