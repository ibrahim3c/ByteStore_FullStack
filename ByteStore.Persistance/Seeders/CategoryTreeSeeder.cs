using ByteStore.Domain.Entities;
using ByteStore.Persistance.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ByteStore.Persistance.Seeders
{
    public class CategoryTreeSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var _appDbContext = serviceProvider.GetRequiredService<AppDbContext>();

            if (!await _appDbContext.Set<CategoryTree>().AnyAsync())
            {
                var categories = new List<CategoryTree>
            {
                new CategoryTree
                {
                    Name = "Laptops",
                    Description = "Wide range of laptops for personal and professional use",
                    SubCategories = new List<CategoryTree>
                    {
                        new CategoryTree { Name = "Gaming Laptops", Description = "High performance laptops for gaming"},
                        new CategoryTree { Name = "Business Laptops", Description = "Laptops designed for business and productivity" },
                        new CategoryTree { Name = "Ultrabooks", Description = "Lightweight and portable laptops" }
                    }
                },
                new CategoryTree
                {
                    Name = "Smartphones",
                    Description = "Latest smartphones with advanced features",
                    SubCategories = new List<CategoryTree>
                    {
                        new CategoryTree { Name = "Android Phones", Description = "Smartphones running Android OS" },
                        new CategoryTree { Name = "iPhones", Description = "Apple iPhones" }
                    }
                },
                new CategoryTree
                {
                    Name = "Tablets",
                    Description = "Portable tablets for work and entertainment",
                    SubCategories = new List<CategoryTree>
                    {
                        new CategoryTree { Name = "Android Tablets", Description = "Tablets running Android OS" },
                        new CategoryTree { Name = "iPads", Description = "Apple iPads" }
                    }
                },
                new CategoryTree
                {
                    Name = "Accessories",
                    Description = "Electronic accessories and peripherals",
                    SubCategories = new List<CategoryTree>
                    {
                        new CategoryTree { Name = "Headphones", Description = "Wired and wireless headphones" },
                        new CategoryTree { Name = "Keyboards", Description = "Mechanical and wireless keyboards" },
                        new CategoryTree { Name = "Mice", Description = "Gaming and regular mice" },
                        new CategoryTree { Name = "Chargers", Description = "Chargers and adapters" }
                    }
                }
            };

                await _appDbContext.Set<CategoryTree>().AddRangeAsync(categories);
                await _appDbContext.SaveChangesAsync();
            }
        }
    }
}