using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Database.EntityTypeConfiguration
{
    public class OrderTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // primary key
            builder.HasKey(p => p.Id)
                   .HasName("id_order_pk");

            // table name
            builder.ToTable("orders");

            // properties
            builder.Property(p => p.Id)
                   .IsRequired()
                   .HasColumnType("uuid")
                   .HasColumnName("id");

            builder.Property(p => p.CustomerId)
                   .IsRequired()
                   .HasColumnType("uuid")
                   .HasColumnName("customer_id");

            builder.Property(p => p.OrderDate)
                   .IsRequired()
                   .HasColumnName("order_date");

            builder.Property(p => p.ShipmentDate)
                   .HasColumnName("shipment_date");

            builder.Property(p => p.OrderNumber)
                   .HasColumnName("order_number");

            builder.Property(p => p.OrderStatus)
                   .HasMaxLength(100)
                   .HasColumnName("order_status");

            // foreign keys
            builder.HasOne(p => p.Customer)
                   .WithMany(p => p.Orders)
                   .HasForeignKey(p => p.CustomerId)
                   .HasConstraintName("1tomany_customer_to_orders_fk");

            builder.HasMany(p => p.OrderElements)
                   .WithOne(p => p.Order)
                   .HasForeignKey(p => p.OrderId)
                   .HasConstraintName("manyto1_order_elements_to_order_fk");
        }
    }
}
