using ByteStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByteStore.Persistance.Configurations
{
    public class CustomerConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.FirstName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.LastName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.DateOfBirth)
                   .HasColumnType("date");

            builder.HasOne(u => u.AppUser)
                    .WithOne()
                    .HasForeignKey<Customer>(up => up.AppUserId);

            builder.HasQueryFilter(c => !c.IsDeleted);
        }
    }
}
