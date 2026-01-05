using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Database.EntityTypeConfiguration
{
    public class UserTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
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
                   .HasColumnType("uuid");

            builder.Property(p => p.Username)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.Password)
                   .IsRequired();

            builder.Property(p => p.Salt)
                   .IsRequired();

            builder.Property(p => p.Role)
                   .IsRequired();

            // indexes
            builder.HasIndex(p => p.Username)
                   .IsUnique();

            // foreign keys
            builder.HasOne(p => p.Customer)
                   .WithOne(p => p.User)
                   .HasForeignKey<User>(p => p.CustomerId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
