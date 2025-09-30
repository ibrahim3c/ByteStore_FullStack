using ByteStore.Domain.Entities;
using ByteStore.Domain.Repositories;
using ByteStore.Persistance.Database;
using ByteStore.Persistance.Specifications;
using Microsoft.EntityFrameworkCore;

namespace ByteStore.Persistance.Repositories
{
    internal class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}