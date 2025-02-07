using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Database.EntityTypeConfiguration
{
    public class UserTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // primary key
            builder.HasKey(p => p.Id)
                   .HasName("id_user_pk");

            // table name
            builder.ToTable("users_table");

            // properties
            builder.Property(p => p.Id)
                   .HasColumnName("id");

            builder.Property(p => p.CustomerId)
                   .HasColumnType("uuid")
                   .HasColumnName("customer_id");

            builder.Property(p => p.Username)
                   .IsRequired()
                   .HasMaxLength(100)
                   .HasColumnName("username");

            builder.Property(p => p.Password)
                   .IsRequired()
                   .HasColumnName("password");

            builder.Property(p => p.Salt)
                   .IsRequired()
                   .HasColumnName("salt");

            builder.Property(p => p.Role)
                   .IsRequired()
                   .HasColumnName("role");

            // indexes
            builder.HasIndex(p => p.Username)
                   .IsUnique();

            // foreign keys
            builder.HasOne(p => p.Customer)
                   .WithOne(p => p.User)
                   .HasForeignKey<User>(p => p.CustomerId)
                   .HasConstraintName("1to1_customer_to_user_fk");
        }
    }
}
