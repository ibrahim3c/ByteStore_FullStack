using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using BytStore.Application.Helpers;
namespace BytStore.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure ImageKitOptions using the "ImageKit" section from configuration
            services.Configure<ImageKitOptions>(configuration.GetSection("ImageKitOptions"));

            return services;
        }
    }
}
