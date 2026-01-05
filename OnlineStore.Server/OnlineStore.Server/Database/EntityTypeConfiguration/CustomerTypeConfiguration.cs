using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Database.EntityTypeConfiguration
{
    public class CustomerTypeConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            // properties
            builder.Property(p => p.Id)
                   .IsRequired()
                   .ValueGeneratedOnAdd()
                   .HasColumnType("uuid");

            builder.Property(p => p.CreatedAt)
                   .IsRequired()
                   .HasColumnType("timestamp");

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(p => p.Code)
                   .IsRequired()
                   .HasMaxLength(9);

            builder.Property(p => p.Address)
                   .HasMaxLength(255);

            builder.Property(p => p.Discount);

            // indexes
            builder.HasIndex(p => p.Code)
                   .IsUnique();

            // foreign keys
            builder.HasOne(p => p.User)
                   .WithOne(p => p.Customer)
                   .HasForeignKey<Customer>(p => p.Id)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(p => p.Orders)
                   .WithOne(p => p.Customer)
                   .HasForeignKey(p => p.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
