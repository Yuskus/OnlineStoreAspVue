using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Server.Database.Entities;
using OnlineStore.Server.Extensions.BCL.Structures;
using System.Globalization;

namespace OnlineStore.Server.Database.EntityTypeConfiguration
{
    public class OrderTypeConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // properties
            builder.Property(p => p.Id)
                   .IsRequired()
                   .ValueGeneratedOnAdd()
                   .HasColumnType("uuid");

            builder.Property(p => p.CreatedAt)
                   .IsRequired()
                   .HasColumnType("timestamp");

            builder.Property(p => p.CustomerId)
                   .IsRequired()
                   .HasColumnType("uuid");

            builder.Property(p => p.OrderDate)
                   .IsRequired();

            builder.Property(p => p.ShipmentDate);

            builder.Property(p => p.OrderNumber);

            builder.Property(p => p.OrderStatus)
                   .HasMaxLength(100);

            // foreign keys
            builder.HasOne(p => p.Customer)
                   .WithMany(p => p.Orders)
                   .HasForeignKey(p => p.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.OrderElements)
                   .WithOne(p => p.Order)
                   .HasForeignKey(p => p.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
