using ByteStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByteStore.Persistance.Configurations
{
    public class OrderConfig : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");
                
            builder.HasKey(o => o.Id);

            builder.Property(o => o.TotalAmount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired();

            builder.Property(o => o.Status)
                   .IsRequired();


            builder.HasOne(o => o.Customer)
                   .WithMany(c => c.Orders)
                   .HasForeignKey(o => o.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);

            
            //one address has many orders
            //زي مثلا العميل عمل 3 طلبات مختلفة في 3 أيام، وكلهم استخدموا نفس العنوان للشحن 
            builder.HasOne(o => o.ShippingAddress)
                   .WithMany() 
                   .HasForeignKey(o => o.ShippingAddressId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(o => o.BillingAddress)
                   .WithMany()
                   .HasForeignKey(o => o.BillingAddressId)
                   .OnDelete(DeleteBehavior.Restrict);


        }
    }
}
