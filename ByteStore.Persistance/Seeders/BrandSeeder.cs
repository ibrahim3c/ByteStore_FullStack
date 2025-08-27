using ByteStore.Domain.Entities;
using ByteStore.Persistance.Database;
using Microsoft.EntityFrameworkCore;

namespace ByteStore.Persistance.Seeders
{
    public class BrandSeeder
    {
        private readonly AppDbContext _appDbContext;

        public BrandSeeder(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task SeedAsync()
        {
            if (!await _appDbContext.Set<Brand>().AnyAsync())
            {
                var brands = new List<Brand>
            {
                // Smartphones & Tablets
                new Brand { Name = "Apple", Description = "iPhone, iPad, Mac, accessories", LogoUrl = "/images/brands/apple.png" },
                new Brand { Name = "Samsung", Description = "Smartphones, tablets, TVs, home appliances", LogoUrl = "/images/brands/samsung.png" },
                new Brand { Name = "Huawei", Description = "Smartphones, networking devices, tablets", LogoUrl = "/images/brands/huawei.png" },
                new Brand { Name = "Xiaomi", Description = "Smartphones, tablets, wearables, smart home devices", LogoUrl = "/images/brands/xiaomi.png" },
                new Brand { Name = "Oppo", Description = "Smartphones and accessories", LogoUrl = "/images/brands/oppo.png" },
                new Brand { Name = "Vivo", Description = "Smartphones and accessories", LogoUrl = "/images/brands/vivo.png" },
                new Brand { Name = "Realme", Description = "Smartphones and smart devices", LogoUrl = "/images/brands/realme.png" },
                new Brand { Name = "OnePlus", Description = "Premium smartphones and accessories", LogoUrl = "/images/brands/oneplus.png" },
                new Brand { Name = "Google", Description = "Pixel smartphones and devices", LogoUrl = "/images/brands/google.png" },

                // Laptops & Computers
                new Brand { Name = "Dell", Description = "Laptops, desktops, accessories", LogoUrl = "/images/brands/dell.png" },
                new Brand { Name = "HP", Description = "Laptops, desktops, printers", LogoUrl = "/images/brands/hp.png" },
                new Brand { Name = "Lenovo", Description = "Laptops, desktops, tablets", LogoUrl = "/images/brands/lenovo.png" },
                new Brand { Name = "Asus", Description = "Laptops, gaming laptops, PC components", LogoUrl = "/images/brands/asus.png" },
                new Brand { Name = "Acer", Description = "Laptops, monitors, accessories", LogoUrl = "/images/brands/acer.png" },
                new Brand { Name = "MSI", Description = "Gaming laptops, motherboards, GPUs", LogoUrl = "/images/brands/msi.png" },
                new Brand { Name = "Razer", Description = "Gaming laptops and accessories", LogoUrl = "/images/brands/razer.png" },
                new Brand { Name = "Microsoft", Description = "Surface laptops, tablets, software", LogoUrl = "/images/brands/microsoft.png" },

                // Accessories & Electronics
                new Brand { Name = "Sony", Description = "Electronics, PlayStation, TVs, headphones", LogoUrl = "/images/brands/sony.png" },
                new Brand { Name = "Logitech", Description = "Keyboards, mice, webcams, gaming gear", LogoUrl = "/images/brands/logitech.png" },
                new Brand { Name = "Corsair", Description = "PC components, gaming peripherals", LogoUrl = "/images/brands/corsair.png" },
                new Brand { Name = "Kingston", Description = "RAM, SSDs, storage devices", LogoUrl = "/images/brands/kingston.png" },
                new Brand { Name = "SanDisk", Description = "Flash drives, SD cards, storage devices", LogoUrl = "/images/brands/sandisk.png" },
                new Brand { Name = "Seagate", Description = "Hard drives, SSDs, storage solutions", LogoUrl = "/images/brands/seagate.png" },
                new Brand { Name = "Western Digital", Description = "Storage devices and cloud solutions", LogoUrl = "/images/brands/wd.png" },
                new Brand { Name = "Anker", Description = "Chargers, cables, power banks, accessories", LogoUrl = "/images/brands/anker.png" },
                new Brand { Name = "Beats", Description = "Headphones and audio devices", LogoUrl = "/images/brands/beats.png" },
                new Brand { Name = "Bose", Description = "Headphones and audio systems", LogoUrl = "/images/brands/bose.png" }
            };

                await _appDbContext.Set<Brand>().AddRangeAsync(brands);
                await _appDbContext.SaveChangesAsync();
            }
        }
    }
}