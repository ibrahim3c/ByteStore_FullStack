using ByteStore.Domain.Entities;
using ByteStore.Persistance.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ByteStore.Persistance.Seeders
{
    public class CategorySeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var _appDbContext = serviceProvider.GetRequiredService<AppDbContext>();

            if (!await _appDbContext.Set<Category>().AnyAsync())
            {
                var categories = new List<Category>
                {
                    new Category
                    {
                        Name = "Laptops",
                        Description = "Wide range of laptops for personal and professional use"
                    },
                    new Category
                    {
                        Name = "Smartphones",
                        Description = "Latest smartphones with advanced features"
                    },
                    new Category
                    {
                        Name = "Tablets",
                        Description = "Portable tablets for work and entertainment"
                    },
                    new Category
                    {
                        Name = "Accessories",
                        Description = "Electronic accessories and peripherals"
                    },
                    new Category
                    {
                        Name = "Headphones",
                        Description = "Wired and wireless headphones"
                    },
                    new Category
                    {
                        Name = "Keyboards",
                        Description = "Mechanical and wireless keyboards"
                    },
                    new Category
                    {
                        Name = "Mice",
                        Description = "Gaming and regular mice"
                    },
                    new Category
                    {
                        Name = "Chargers",
                        Description = "Chargers and adapters"
                    }
                };

                await _appDbContext.Set<Category>().AddRangeAsync(categories);
                await _appDbContext.SaveChangesAsync();
            }
        }
    }
}