using ByteStore.Domain.Entities;
using ByteStore.Domain.Repositories;
using ByteStore.Persistance.Database;

namespace ByteStore.Persistance.Repositories
{
    internal class CategoryTreeRepository : BaseRepository<CategoryTree>, ICategoryTreeRepository
    {
        public CategoryTreeRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
