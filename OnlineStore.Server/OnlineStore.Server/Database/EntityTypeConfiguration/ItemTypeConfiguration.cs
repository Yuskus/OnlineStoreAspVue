using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Server.Database.Entities;
using OnlineStore.Server.Extensions.BCL.Structures;
using System.Globalization;

namespace OnlineStore.Server.Database.EntityTypeConfiguration
{
    public class ItemTypeConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            // properties
            builder.Property(p => p.Id)
                   .IsRequired()
                   .ValueGeneratedOnAdd()
                   .HasColumnType("uuid");

            builder.Property(p => p.CreatedAt)
                   .IsRequired()
                   .HasColumnType("timestamp");

            builder.Property(p => p.Code)
                   .IsRequired()
                   .HasMaxLength(12);

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(p => p.Price)
                   .HasColumnType("decimal(10, 2)");

            builder.Property(p => p.Category)
                   .HasMaxLength(255);

            // indexes
            builder.HasIndex(p => p.Code)
                   .IsUnique();

            // foreign keys
            builder.HasMany(p => p.OrderElements)
                   .WithOne(p => p.Item)
                   .HasForeignKey(p => p.ItemId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
