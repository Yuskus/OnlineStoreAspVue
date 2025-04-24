using Microsoft.EntityFrameworkCore;
using OnlineStore.Server.Database.Entities;
using System.Reflection;

namespace OnlineStore.Server.Database.Context
{
    public class OnlineStoreDbContext(DbContextOptions<OnlineStoreDbContext> options) : DbContext(options)
    {
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderElement> OrderElements { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
