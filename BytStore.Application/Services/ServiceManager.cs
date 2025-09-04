using BytStore.Application.IServices;

namespace BytStore.Application.Services
{
    internal sealed class ServiceManager : IServiceManager
    {
        private readonly Lazy<IAuthService> _authService;
        private readonly Lazy<IBrandService> _brandService;
        private readonly Lazy<ICategoryService> _categoryService;
        private readonly Lazy<IProductService> _productService;
        private readonly Lazy<IOrderService> _orderService;
        private readonly Lazy<ICustomerService> _customerService;

        // someone said using lazy loading

        public ServiceManager(
            IAuthService authService,
            IBrandService brandService,
            ICategoryService categoryService,
            IProductService productService,
            IOrderService orderService,
            ICustomerService customerService)
        {
            _authService = new Lazy<IAuthService>(() => authService);
            _brandService = new Lazy<IBrandService>(() => brandService);
            _categoryService = new Lazy<ICategoryService>(() => categoryService);
            _productService = new Lazy<IProductService>(() => productService);
            _orderService = new Lazy<IOrderService>(() => orderService);
            _customerService= new Lazy<ICustomerService>(() => customerService);
        }

        public IAuthService AuthService => _authService.Value;
        public IBrandService BrandService => _brandService.Value;
        public ICategoryService CategoryService => _categoryService.Value;
        public IProductService ProductService => _productService.Value;
        public IOrderService OrderService => _orderService.Value;
        public ICustomerService CustomerService => _customerService.Value;
    }
}
