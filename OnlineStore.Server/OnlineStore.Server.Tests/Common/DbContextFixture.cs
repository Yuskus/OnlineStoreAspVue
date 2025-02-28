using OnlineStore.Server.Authorization.Utilities;
using OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Tests.Common
{
    public class DbContextFixture : IDisposable
    {
        public FakeDbContext Context { get; set; }

        private static int counter;
        private readonly object locker = new();
        
        private bool disposed = false;

        public DbContextFixture()
        {
            Context = FakeDbContextFactory.Create();
        }

        private int UniqInteger
        {
            get
            {
                lock (locker)
                {
                    return counter++;
                }
            }
        }

        public Guid[] AddCustomers(int capacity)
        {
            var customers = new Customer[capacity];

            for (int i = 0; i < capacity; i++)
            {
                customers[i] = new()
                {
                    Name = Guid.NewGuid().ToString(),
                    Code = $"{1000 + UniqInteger}-2000"
                };
            }

            Context.Customers.AddRange(customers);
            Context.SaveChanges();

            return customers.Select(x => x.Id).ToArray();
        }

        public Guid[] AddItems(int capacity)
        {
            var items = new Item[capacity];

            for (int i = 0; i < capacity; i++)
            {
                items[i] = new()
                {
                    Code = $"01-{1000 + UniqInteger}-AB23",
                    Name = $"TestItemName{i}",
                    Category = $"Some Category {i % 4}"
                };
            }

            Context.Items.AddRange(items);
            Context.SaveChanges();

            return items.Select(x => x.Id).ToArray();
        }

        public Guid[] AddOrders(int capacity, Guid[] customers)
        {
            var orders = new Order[capacity];

            for (int i = 0; i < capacity; i++)
            {
                orders[i] = new()
                {
                    CustomerId = customers[i % customers.Length],
                    OrderDate = DateOnly.FromDateTime(DateTime.Now),
                    OrderNumber = i,
                    OrderStatus = i % 2 == 0 ? "new" : "basket"
                };
            }

            Context.Orders.AddRange(orders);
            Context.SaveChanges();

            return orders.Select(x => x.Id).ToArray();
        }

        public Guid[] AddOrderElements(int capacity, Guid[] orders, Guid[] items)
        {
            var elements = new OrderElement[capacity];

            for (int i = 0; i < capacity; i++)
            {
                elements[i] = new()
                {
                    OrderId = orders[i % orders.Length],
                    ItemId = items[i % items.Length],
                    ItemsCount = i % 3,
                    ItemPrice = 100 + i
                };
            }

            Context.OrderElements.AddRange(elements);
            Context.SaveChanges();

            return elements.Select(x => x.Id).ToArray();
        }

        public string[] AddUsers(Guid[] customers)
        {
            int capacity = customers.Length * 2;

            var users = new User[capacity];

            for (int i = 0; i < capacity; i++)
            {
                (byte[] hash, byte[] salt) = Hasher.CreatePasswordHash($"TestPassword{i}");

                users[i] = new()
                {
                    Username = $"TestUsername{i}",
                    Password = hash,
                    Salt = salt,
                    CustomerId = i < customers.Length ? customers[i] : null, // half of customers
                    Role = i >= customers.Length ? (int)UserRole.Manager : (int)UserRole.User, // half of managers
                };
            }

            Context.Users.AddRange(users);
            Context.SaveChanges();

            return users.Select(x => x.Username).ToArray();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed) return;
            if (disposing)
            {
                FakeDbContextFactory.Delete(Context);
            }
            disposed = true;
        }

        ~DbContextFixture()
        {
            Dispose(false);
        }
    }

    [CollectionDefinition("DatabaseCollection")]
    public class DatabaseCollection : ICollectionFixture<DbContextFixture> { }
}
