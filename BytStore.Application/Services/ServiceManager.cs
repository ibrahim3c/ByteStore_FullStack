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
        private readonly Lazy<IShoppingCartService> _shoppingCartService;

        private readonly Lazy<IEmailService> _emailService;
        private readonly Lazy<IImageService> _imageService;
        private readonly Lazy<IRolesService> _roleService;
        private readonly Lazy<IUserService> _userService;

        // someone said using lazy loading

        public ServiceManager(
            IAuthService authService,
            IBrandService brandService,
            ICategoryService categoryService,
            IProductService productService,
            IOrderService orderService,
            ICustomerService customerService,
            IShoppingCartService shoppingCartService,
            IEmailService emailService,
            IImageService imageService,
            IUserService userService,
            IRolesService roleService)
        {   
            _authService = new Lazy<IAuthService>(() => authService);
            _brandService = new Lazy<IBrandService>(() => brandService);
            _categoryService = new Lazy<ICategoryService>(() => categoryService);
            _productService = new Lazy<IProductService>(() => productService);
            _orderService = new Lazy<IOrderService>(() => orderService);
            _customerService = new Lazy<ICustomerService>(() => customerService);
            _shoppingCartService = new Lazy<IShoppingCartService>(() => shoppingCartService);
            _emailService = new Lazy<IEmailService>(() => emailService);
            _imageService = new Lazy<IImageService>(() => imageService);
            _roleService = new Lazy<IRolesService>(() => roleService);
            _userService = new Lazy<IUserService>(() => userService);
        }


        public IAuthService AuthService => _authService.Value;
        public IBrandService BrandService => _brandService.Value;
        public ICategoryService CategoryService => _categoryService.Value;
        public IProductService ProductService => _productService.Value;
        public IOrderService OrderService => _orderService.Value;
        public ICustomerService CustomerService => _customerService.Value;
        public IShoppingCartService ShoppingCartService => _shoppingCartService.Value;
        public IEmailService EmailService => _emailService.Value;
        public IImageService ImageService => _imageService.Value;
        public IRolesService RoleService => _roleService.Value;
        public IUserService UserService => _userService.Value;
    }
}
