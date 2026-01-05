using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Database.EntityTypeConfiguration
{
    public class OrderElementTypeConfiguration : IEntityTypeConfiguration<OrderElement>
    {
        public void Configure(EntityTypeBuilder<OrderElement> builder)
        {
            // properties
            builder.Property(p => p.Id)
                   .IsRequired()
                   .ValueGeneratedOnAdd()
                   .HasColumnType("uuid");

            builder.Property(p => p.CreatedAt)
                   .IsRequired()
                   .HasColumnType("timestamp");

            builder.Property(p => p.OrderId)
                   .IsRequired()
                   .HasColumnType("uuid");

            builder.Property(p => p.ItemId)
                   .IsRequired()
                   .HasColumnType("uuid");

            builder.Property(p => p.ItemsCount)
                   .IsRequired();

            builder.Property(p => p.ItemPrice)
                   .IsRequired()
                   .HasColumnType("decimal(10, 2)");

            // foreign keys
            builder.HasOne(p => p.Order)
                   .WithMany(p => p.OrderElements)
                   .HasForeignKey(p => p.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Item)
                   .WithMany(p => p.OrderElements)
                   .HasForeignKey(p => p.ItemId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
