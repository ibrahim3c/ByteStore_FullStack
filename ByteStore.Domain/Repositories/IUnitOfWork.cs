namespace ByteStore.Domain.Repositories
{
    public interface IUnitOfWork:IDisposable
    {
        IProductRepository ProductRepository { get; }
        ICategoryTreeRepository CategoryTreeRepository { get; }
        IOrderRepository OrderRepository { get; }
        ICustomerRepository CustomerRepository { get; }

        IBaseRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

        Task<int> SaveChangesAsync();

    }
}
