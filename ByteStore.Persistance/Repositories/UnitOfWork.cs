using ByteStore.Domain.Repositories;
using ByteStore.Persistance.Database;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace ByteStore.Persistance.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext appDbContext;
        private readonly IConnectionMultiplexer connectionMultiplexer;

        public UnitOfWork(AppDbContext appDbContext,IConnectionMultiplexer connectionMultiplexer)
        {
            this.appDbContext = appDbContext;
            this.connectionMultiplexer = connectionMultiplexer;
            ProductRepository = new ProductRepository(appDbContext);
            CategoryRepository = new CategoryRepository(appDbContext);
            OrderRepository = new OrderRepository(appDbContext);
            ShoppingCartRepository = new ShoppingCartRepository(connectionMultiplexer);
            CustomerRepository = new CustomerRepository(appDbContext);
        }
        public IProductRepository ProductRepository { get; }
        public ICategoryRepository CategoryRepository { get; }
        public IOrderRepository OrderRepository { get; }
        public IShoppingCartRepository ShoppingCartRepository { get; }
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
