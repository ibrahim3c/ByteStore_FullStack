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
                        Description = "Wide range of laptops for personal and professional use",
                        // Image: A silver laptop open on a desk
                        ImageUrl = "https://images.unsplash.com/photo-1496181133206-80ce9b88a853?auto=format&fit=crop&w=800&q=80"
                    },
                    new Category
                    {
                        Name = "Smartphones",
                        Description = "Latest smartphones with advanced features",
                        // Image: A modern smartphone screen
                        ImageUrl = "https://images.unsplash.com/photo-1511707171634-5f897ff02aa9?auto=format&fit=crop&w=800&q=80"
                    },
                    new Category
                    {
                        Name = "Tablets",
                        Description = "Portable tablets for work and entertainment",
                        // Image: A tablet with a stylus
                        ImageUrl = "https://images.unsplash.com/photo-1544244015-0df4b3ffc6b0?auto=format&fit=crop&w=800&q=80"
                    },
                    new Category
                    {
                        Name = "Accessories",
                        Description = "Electronic accessories and peripherals",
                        // Image: A flat lay of tech accessories (hard drive, cables, mouse)
                        ImageUrl = "https://images.unsplash.com/photo-1527443224154-c4a3942d3acf?auto=format&fit=crop&w=800&q=80"
                    },
                    new Category
                    {
                        Name = "Headphones",
                        Description = "Wired and wireless headphones",
                        // Image: High-quality over-ear headphones
                        ImageUrl = "https://images.unsplash.com/photo-1505740420928-5e560c06d30e?auto=format&fit=crop&w=800&q=80"
                    },
                   new Category
                    {
                        Name = "Keyboards",
                        Description = "Mechanical and wireless keyboards",
                        // FIXED LINK: A high quality mechanical keyboard
                        ImageUrl = "https://images.unsplash.com/photo-1595225476474-87563907a212?auto=format&fit=crop&w=800&q=80"
                    },
                    new Category
                    {
                        Name = "Mice",
                        Description = "Gaming and regular mice",
                        // Image: A computer mouse on a pad
                        ImageUrl = "https://images.unsplash.com/photo-1527864550417-7fd91fc51a46?auto=format&fit=crop&w=800&q=80"
                    },
                    new Category
                    {
                        Name = "Chargers",
                        Description = "Chargers and adapters",
                        ImageUrl = "https://images.pexels.com/photos/20360345/pexels-photo-20360345.jpeg"
                    }

                };

                await _appDbContext.Set<Category>().AddRangeAsync(categories);
                await _appDbContext.SaveChangesAsync();
            }
        }
    }
}