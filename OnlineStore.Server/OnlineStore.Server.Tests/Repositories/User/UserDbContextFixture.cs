using OnlineStore.Server.Authorization.Utilities;
using OnlineStore.Server.Database.Entities;
using OnlineStore.Server.Tests.Common;
using Entity = OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Tests.Repositories.User
{
    public class UserDbContextFixture : DbContextFixture
    {
        public string ManagerUsername_ForUpdate { get; private set; } = "ManagerForUpdate";
        public string CustomerUsername_ForUpdate { get; private set; } = "CustomerForUpdate";
        public string ManagerUsername_ForDelete { get; private set; } = "ManagerForDelete";
        public string CustomerUsername_ForDelete { get; private set; } = "CustomerForDelete";
        public string Username_Unexist { get; private set; } = "SomeUnexistUser";
        public Guid CustomerId_ForRegister { get; private set; }
        public string CustomerName_ForRegister { get; private set; } = "TestName100";
        public string CustomerCode_ForRegister { get; private set; } = "1234-2000";
        public int UsersTotalCount { get; private set; }

        public UserDbContextFixture()
        {
            Initialize();
        }

        public void Initialize(int capacity = 30)
        {
            // заполнение БД (customers for users)

            var customers = new Entity.Customer[capacity / 2];

            for (int i = 0; i < capacity / 2; i++)
            {
                customers[i] = new() { Name = $"TestName{i}", Code = $"{2000 + i}-2000" };
            }

            Context.Customers.AddRange(customers);
            Context.SaveChanges();

            // заполнение БД (users)

            var users = new Entity.User[capacity];

            byte[] hash, salt;

            for (int i = 0; i < capacity; i++)
            {
                (hash, salt) = Hasher.CreatePasswordHash($"testPassword{i}");

                users[i] = new Entity.User { Username = $"TestUsername{i}", Password = hash, Salt = salt };

                if (i < capacity / 2) // then manager
                {
                    users[i].Role = (int)UserRole.Manager;
                }
                else // then customer
                {
                    users[i].CustomerId = customers[i - capacity / 2].Id;
                }
            }

            Context.Users.AddRange(users);

            var customerForRegister = new Entity.Customer { Name = CustomerName_ForRegister, Code = CustomerCode_ForRegister };
            Context.Customers.Add(customerForRegister);
            Context.SaveChanges();
            CustomerId_ForRegister = customerForRegister.Id;

            (hash, salt) = Hasher.CreatePasswordHash($"testPassword_UPD1");
            Context.Users.Add(new() { Username = ManagerUsername_ForUpdate, Password = hash, Salt = salt, Role = (int)UserRole.Manager });
            UsersTotalCount++;

            (hash, salt) = Hasher.CreatePasswordHash($"testPassword_UPD2");
            Context.Users.Add(new() { Username = CustomerUsername_ForUpdate, Password = hash, Salt = salt });
            UsersTotalCount++;

            (hash, salt) = Hasher.CreatePasswordHash($"testPassword_DEL1");
            Context.Users.Add(new() { Username = ManagerUsername_ForDelete, Password = hash, Salt = salt, Role = (int)UserRole.Manager });
            UsersTotalCount++;

            (hash, salt) = Hasher.CreatePasswordHash($"testPassword_DEL2");
            Context.Users.Add(new() { Username = CustomerUsername_ForDelete, Password = hash, Salt = salt });
            UsersTotalCount++;

            Context.SaveChanges();

            // инициализация переменных для тестов

            UsersTotalCount += capacity;
        }
    }
}
