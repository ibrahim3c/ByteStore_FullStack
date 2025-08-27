using ByteStore.Domain.Entities;
using ByteStore.Persistance.Database;
using Microsoft.EntityFrameworkCore;

namespace ByteStore.Persistance.Seeders
{
    public class CategorySeeder
    {
        private readonly AppDbContext _appDbContext;

        public CategorySeeder(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task SeedAsync()
        {
            if (!await _appDbContext.Set<Category>().AnyAsync())
            {
                var categories = new List<Category>
            {
                new Category
                {
                    Name = "Laptops",
                    Description = "Wide range of laptops for personal and professional use",
                    ImageUrl = "/images/categories/laptops.jpg",
                    SubCategories = new List<Category>
                    {
                        new Category { Name = "Gaming Laptops", Description = "High performance laptops for gaming", ImageUrl = "/images/categories/gaming-laptops.jpg" },
                        new Category { Name = "Business Laptops", Description = "Laptops designed for business and productivity", ImageUrl = "/images/categories/business-laptops.jpg" },
                        new Category { Name = "Ultrabooks", Description = "Lightweight and portable laptops", ImageUrl = "/images/categories/ultrabooks.jpg" }
                    }
                },
                new Category
                {
                    Name = "Smartphones",
                    Description = "Latest smartphones with advanced features",
                    ImageUrl = "/images/categories/smartphones.jpg",
                    SubCategories = new List<Category>
                    {
                        new Category { Name = "Android Phones", Description = "Smartphones running Android OS", ImageUrl = "/images/categories/android.jpg" },
                        new Category { Name = "iPhones", Description = "Apple iPhones", ImageUrl = "/images/categories/iphone.jpg" }
                    }
                },
                new Category
                {
                    Name = "Tablets",
                    Description = "Portable tablets for work and entertainment",
                    ImageUrl = "/images/categories/tablets.jpg",
                    SubCategories = new List<Category>
                    {
                        new Category { Name = "Android Tablets", Description = "Tablets running Android OS", ImageUrl = "/images/categories/android-tablets.jpg" },
                        new Category { Name = "iPads", Description = "Apple iPads", ImageUrl = "/images/categories/ipads.jpg" }
                    }
                },
                new Category
                {
                    Name = "Accessories",
                    Description = "Electronic accessories and peripherals",
                    ImageUrl = "/images/categories/accessories.jpg",
                    SubCategories = new List<Category>
                    {
                        new Category { Name = "Headphones", Description = "Wired and wireless headphones", ImageUrl = "/images/categories/headphones.jpg" },
                        new Category { Name = "Keyboards", Description = "Mechanical and wireless keyboards", ImageUrl = "/images/categories/keyboards.jpg" },
                        new Category { Name = "Mice", Description = "Gaming and regular mice", ImageUrl = "/images/categories/mice.jpg" },
                        new Category { Name = "Chargers", Description = "Chargers and adapters", ImageUrl = "/images/categories/chargers.jpg" }
                    }
                }
            };

                await _appDbContext.Set<Category>().AddRangeAsync(categories);
                await _appDbContext.SaveChangesAsync();
            }
        }
    }
}