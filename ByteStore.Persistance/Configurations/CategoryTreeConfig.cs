using ByteStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ByteStore.Persistance.Configurations
{
    public class CategoryTreeConfig : IEntityTypeConfiguration<CategoryTree>
    {
        public void Configure(EntityTypeBuilder<CategoryTree> builder)
        {
            builder.ToTable("CategoryTrees");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                   .IsRequired()
                   .HasMaxLength(150);

            builder.Property(c => c.Description)
                   .HasMaxLength(500);

            // Self-referencing relationship: One Category -> Many SubCategories
            builder.HasOne(c => c.ParentCategory)
                   .WithMany(c => c.SubCategories)
                   .HasForeignKey(c => c.ParentCategoryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
