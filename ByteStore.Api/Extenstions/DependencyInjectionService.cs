using ByteStore.Persistance;
using BytStore.Application;

namespace ByteStore.Api.Extenstions
{
    public  static class DependencyInjectionService
    {
        public static IServiceCollection AddDependencyInjectionService(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddApplicationLayer(configuration);
            services.AddPersistanceLayer(configuration);

            return services;
        }
    }
}
