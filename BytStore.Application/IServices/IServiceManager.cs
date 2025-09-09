namespace BytStore.Application.IServices
{
    public interface IServiceManager
    {
        IAuthService AuthService { get; }
        IBrandService BrandService { get; }
        ICategoryService CategoryService { get; }
        IProductService ProductService { get; }
        IOrderService OrderService { get; }
        ICustomerService CustomerService { get; }
        IShoppingCartService ShoppingCartService { get; }

    }
}
