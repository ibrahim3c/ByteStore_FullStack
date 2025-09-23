using ByteStore.Domain.Entities;
using ByteStore.Persistance.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ByteStore.Persistance.Seeders
{
    public class BrandSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var _appDbContext = serviceProvider.GetRequiredService<AppDbContext>();
            if (!await _appDbContext.Set<Brand>().AnyAsync())
            {
                var brands = new List<Brand>
            {
                // Smartphones & Tablets
                new Brand { Name = "Apple", Description = "iPhone, iPad, Mac, accessories" },
                new Brand { Name = "Samsung", Description = "Smartphones, tablets, TVs, home appliances" },
                new Brand { Name = "Huawei", Description = "Smartphones, networking devices, tablets" },
                new Brand { Name = "Xiaomi", Description = "Smartphones, tablets, wearables, smart home devices" },
                new Brand { Name = "Oppo", Description = "Smartphones and accessories" },
                new Brand { Name = "Vivo", Description = "Smartphones and accessories" },
                new Brand { Name = "Realme", Description = "Smartphones and smart devices" },
                new Brand { Name = "OnePlus", Description = "Premium smartphones and accessories" },
                new Brand { Name = "Google", Description = "Pixel smartphones and devices" },

                // Laptops & Computers
                new Brand { Name = "Dell", Description = "Laptops, desktops, accessories" },
                new Brand { Name = "HP", Description = "Laptops, desktops, printers" },
                new Brand { Name = "Lenovo", Description = "Laptops, desktops, tablets" },
                new Brand { Name = "Asus", Description = "Laptops, gaming laptops, PC components" },
                new Brand { Name = "Acer", Description = "Laptops, monitors, accessories" },
                new Brand { Name = "MSI", Description = "Gaming laptops, motherboards, GPUs" },
                new Brand { Name = "Razer", Description = "Gaming laptops and accessories" },
                new Brand { Name = "Microsoft", Description = "Surface laptops, tablets, software" },

                // Accessories & Electronics
                new Brand { Name = "Sony", Description = "Electronics, PlayStation, TVs, headphones" },
                new Brand { Name = "Logitech", Description = "Keyboards, mice, webcams, gaming gear" },
                new Brand { Name = "Corsair", Description = "PC components, gaming peripherals" },
                new Brand { Name = "Kingston", Description = "RAM, SSDs, storage devices" },
                new Brand { Name = "SanDisk", Description = "Flash drives, SD cards, storage devices" },
                new Brand { Name = "Seagate", Description = "Hard drives, SSDs, storage solutions" },
                new Brand { Name = "Western Digital", Description = "Storage devices and cloud solutions" },
                new Brand { Name = "Anker", Description = "Chargers, cables, power banks, accessories" },
                new Brand { Name = "Beats", Description = "Headphones and audio devices" },
                new Brand { Name = "Bose", Description = "Headphones and audio systems" }
            };

                await _appDbContext.Set<Brand>().AddRangeAsync(brands);
                await _appDbContext.SaveChangesAsync();
            }
        }
    }
}