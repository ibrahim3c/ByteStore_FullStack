using BytStore.Application.Helpers;
using BytStore.Application.IServices;
using BytStore.Application.Services;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace BytStore.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure ImageKitOptions using the "ImageKit" section from configuration
            services.Configure<ImageKitOptions>(configuration.GetSection("ImageKitOptions"));

            // register the services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IBrandService, BrandService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();

            //  Register all FluentValidation validators
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

            return services;
        }
    }
}
