using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Database.EntityTypeConfiguration
{
    public class CustomerTypeConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            // primary key
            builder.HasKey(p => p.Id)
                   .HasName("id_customer_pk");

            // table name
            builder.ToTable("customers");

            // properties
            builder.Property(p => p.Id)
                   .IsRequired()
                   .HasColumnType("uuid")
                   .HasColumnName("id");

            builder.Property(p => p.Name)
                   .IsRequired()
                   .HasMaxLength(255)
                   .HasColumnName("name");

            builder.Property(p => p.Code)
                   .IsRequired()
                   .HasMaxLength(9)
                   .HasColumnName("code");

            builder.Property(p => p.Address)
                   .HasMaxLength(255)
                   .HasColumnName("address");

            builder.Property(p => p.Discount)
                   .HasColumnName("discount");

            // indexes
            builder.HasIndex(p => p.Code)
                   .IsUnique();

            // foreign keys
            builder.HasOne(p => p.User)
                   .WithOne(p => p.Customer)
                   .HasForeignKey<Customer>(p => p.Id)
                   .HasConstraintName("1to1_user_to_customer_fk");

            builder.HasMany(p => p.Orders)
                   .WithOne(p => p.Customer)
                   .HasForeignKey(p => p.CustomerId)
                   .HasConstraintName("manyto1_orders_to_customer_fk");
        }
    }
}
