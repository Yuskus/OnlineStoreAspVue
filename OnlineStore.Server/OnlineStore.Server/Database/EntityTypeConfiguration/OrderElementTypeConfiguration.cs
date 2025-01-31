using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Database.EntityTypeConfiguration
{
    public class OrderElementTypeConfiguration : IEntityTypeConfiguration<OrderElement>
    {
        public void Configure(EntityTypeBuilder<OrderElement> builder)
        {
            // primary key
            builder.HasKey(p => p.Id)
                   .HasName("id_order_element_pk");

            // table name
            builder.ToTable("order_elements");

            // properties
            builder.Property(p => p.Id)
                   .IsRequired()
                   .HasColumnType("uuid")
                   .HasColumnName("id");

            builder.Property(p => p.OrderId)
                   .IsRequired()
                   .HasColumnType("uuid")
                   .HasColumnName("order_id");

            builder.Property(p => p.ItemId)
                   .IsRequired()
                   .HasColumnType("uuid")
                   .HasColumnName("item_id");

            builder.Property(p => p.ItemsCount)
                   .IsRequired()
                   .HasColumnName("items_count");

            builder.Property(p => p.ItemPrice)
                   .IsRequired()
                   .HasColumnType("decimal(10, 2)")
                   .HasColumnName("item_price");

            // foreign keys
            builder.HasOne(p => p.Order)
                   .WithMany(p => p.OrderElements)
                   .HasForeignKey(p => p.OrderId)
                   .HasConstraintName("1tomany_order_to_order_elements_fk");

            builder.HasOne(p => p.Item)
                   .WithMany(p => p.OrderElements)
                   .HasForeignKey(p => p.ItemId)
                   .HasConstraintName("1tomany_item_to_order_elements_fk");
        }
    }
}
