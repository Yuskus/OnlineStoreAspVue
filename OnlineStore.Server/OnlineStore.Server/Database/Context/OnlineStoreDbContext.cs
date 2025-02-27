using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Database.Entities;
using OnlineStore.Server.Database.EntityTypeConfiguration;

namespace OnlineStore.Server.Database.Context
{
    public class OnlineStoreDbContext : DbContext
    {
        private readonly string? _connectionString;
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderElement> OrderElements { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<User> Users { get; set; }

        public OnlineStoreDbContext() { }
        public OnlineStoreDbContext(DbContextOptions<OnlineStoreDbContext> options) : base(options) { }
        public OnlineStoreDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies().UseNpgsql(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderTypeConfiguration());
            modelBuilder.ApplyConfiguration(new OrderElementTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ItemTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserTypeConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
