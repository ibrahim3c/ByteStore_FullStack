using ByteStore.Domain.Repositories;
using ByteStore.Persistance.Database;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace ByteStore.Persistance.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext appDbContext;

        public UnitOfWork(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
            ProductRepository = new ProductRepository(appDbContext);
            CategoryTreeRepository = new CategoryTreeRepository(appDbContext);;
            CustomerRepository = new CustomerRepository(appDbContext);
        }
        public IProductRepository ProductRepository { get; }
        public ICategoryTreeRepository CategoryTreeRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public ICustomerRepository CustomerRepository { get; }

        public void Dispose()
        {
            appDbContext.Dispose();
        }

        public IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class
        {
            return new BaseRepository<TEntity>(appDbContext);
        }

        public Task<int> SaveChangesAsync()
        {
            return appDbContext.SaveChangesAsync();
        }
    }
}
