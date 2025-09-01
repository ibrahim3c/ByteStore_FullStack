using ByteStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByteStore.Persistance.Configurations
{
    public class AddressConfig : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Street)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(a => a.City)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.State)
                   .HasMaxLength(100);

            builder.Property(a => a.PostalCode)
                   .HasMaxLength(20);

            builder.Property(a => a.Country)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(a => a.IsPrimary)
                   .IsRequired();

            // Enum stored as string
            builder.Property(a => a.AddressType)
                   .HasConversion<string>()
                   .IsRequired();

            builder.HasOne(a => a.Customer)
                   .WithMany(c => c.Addresses)
                   .HasForeignKey(a => a.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
