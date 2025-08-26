namespace ByteStore.Domain.Repositories
{
    public interface IUnitOfWork:IDisposable
    {
        IProductRepository ProductRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        IOrderRepository OrderRepository { get; }
        IShoppingCartRepository ShoppingCartRepository { get; }

        IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        Task<int> SaveChangesAsync();

    }
}
