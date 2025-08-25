using ByteStore.Persistance.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ByteStore.Persistance
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistanceLayer(this IServiceCollection services,IConfiguration configuration)
        {

            #region EFCore
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new ArgumentNullException(nameof(configuration));
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });
            #endregion
            return services;
        }
    }
}
