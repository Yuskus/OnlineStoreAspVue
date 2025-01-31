using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Database.EntityTypeConfiguration
{
    public class ItemTypeConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            // primary key
            builder.HasKey(p => p.Id)
                   .HasName("id_items_pk");

            // table name
            builder.ToTable("items");

            // properties
            builder.Property(p => p.Id)
                   .IsRequired()
                   .HasColumnType("uuid")
                   .HasColumnName("id");

            builder.Property(p => p.Code)
                   .IsRequired()
                   .HasMaxLength(12)
                   .HasColumnName("code");

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(255)
                   .HasColumnName("name");

            builder.Property(p => p.Price)
                   .HasColumnType("decimal(10, 2)")
                   .HasColumnName("price");

            builder.Property(p => p.Category)
                   .HasMaxLength(255)
                   .HasColumnName("category");

            // indexes
            builder.HasIndex(p => p.Code)
                   .IsUnique();

            // foreign keys
            builder.HasMany(p => p.OrderElements)
                   .WithOne(p => p.Item)
                   .HasForeignKey(p => p.ItemId)
                   .HasConstraintName("manyto1_order_elements_to_item_fk");
        }
    }
}
