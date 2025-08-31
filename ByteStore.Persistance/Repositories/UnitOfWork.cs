using ByteStore.Domain.Repositories;
using ByteStore.Persistance.Database;
using Microsoft.EntityFrameworkCore;

namespace ByteStore.Persistance.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext appDbContext;

        public UnitOfWork(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
            ProductRepository = new ProductRepository(appDbContext);
            CategoryRepository = new CategoryRepository(appDbContext);
            OrderRepository = new OrderRepository(appDbContext);
            ShoppingCartRepository = new ShoppingCartRepository(appDbContext);
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
