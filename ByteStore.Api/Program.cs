using ByteStore.Api.Extenstions;
using ByteStore.Persistance.Database;
using ByteStore.Persistance.Seeders;
using System.Threading.Tasks;

namespace ByteStore.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            #region MyConfigs

            builder.Configuration.AddJsonFile("Secret.json", optional: false, reloadOnChange: true);
            builder.Services.AddDependencyInjectionService(builder.Configuration);

            #endregion
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();


            //using (var scope = app.Services.CreateScope())
            //{
            //    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            //    await  new CategorySeeder(dbContext).SeedAsync();
            //    await new BrandSeeder(dbContext).SeedAsync();;
            //}
            app.Run();
        }
    }
}
