using ByteStore.Api.Extenstions;
using ByteStore.Api.Middlewares;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Scalar.AspNetCore;

namespace ByteStore.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().AddApplicationPart(assembly: typeof(Presentation.AssemblyReference).Assembly);
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            #region MyConfigs

            builder.Configuration.AddJsonFile("Secret.json", optional: false, reloadOnChange: true);
            builder.Services.AddDependencyInjectionService(builder.Configuration);
            builder.Services.AddRateLimiting();
            builder.Services.AddHealthCheck(builder.Configuration);

            #endregion
            var app = builder.Build();

            // global exception hanlder
            //app.UseMiddleware<ExceptionHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseRateLimiter();
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();


            //using (var scope = app.Services.CreateScope())
            //{
            //    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            //    await  new CategorySeeder(dbContext).SeedAsync();
            //    await new BrandSeeder(dbContext).SeedAsync();;
            //}

            // it cause problem of more than dbContext was found
            //  Map Health Checks JSON Endpoint => normal health check just api return json response
            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                //This formats the response in a special JSON format that the HealthCheckUI dashboard understands.
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            // Map HealthCheck UI Dashboard
            app.MapHealthChecksUI(options =>
            {
                options.UIPath = "/health-ui"; // Dashboard path
                options.ApiPath = "/health-ui-api"; // API used by the UI
            });


            app.Run();
        }
    }
}
