using ByteStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByteStore.Persistance.Configurations
{
    public class ShoppingCartConfig : IEntityTypeConfiguration<ShoppingCart>
    {
        public void Configure(EntityTypeBuilder<ShoppingCart> builder)
    {
        builder.ToTable("ShoppingCarts");
        builder.HasKey(sc => sc.Id);

        builder.Property(sc => sc.CreatedAt)
               .HasDefaultValueSql("GETUTCDATE()");

        builder.HasOne(sc => sc.Customer)
               .WithOne(c => c.ShoppingCart)
               .HasForeignKey<ShoppingCart>(sc => sc.CustomerId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
}
