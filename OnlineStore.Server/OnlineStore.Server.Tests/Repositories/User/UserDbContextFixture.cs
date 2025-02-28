using OnlineStore.Server.Authorization.Utilities;
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
            UsersTotalCount = capacity;

            Guid[] guids = AddCustomers(capacity / 2); // fk for users
            string[] _ = AddUsers(guids); // fill table for tests

            // add users for update and delete methods
            AddOneUser(ManagerUsername_ForUpdate, "testPassword_UPD1");
            AddOneUser(CustomerUsername_ForUpdate, "testPassword_UPD2");
            AddOneUser(ManagerUsername_ForDelete, "testPassword_DEL1");
            AddOneUser(CustomerUsername_ForDelete, "testPassword_DEL2");

            Context.SaveChanges();

            // customer for register method
            var customerForRegister = new Entity.Customer
            {
                Name = CustomerName_ForRegister,
                Code = CustomerCode_ForRegister
            };

            Context.Customers.Add(customerForRegister);
            Context.SaveChanges();

            CustomerId_ForRegister = customerForRegister.Id;
        }

        private void AddOneUser(string username, string password)
        {
            (byte[] hash, byte[] salt) = Hasher.CreatePasswordHash(password);
            Context.Users.Add(new() { Username = username, Password = hash, Salt = salt });
            UsersTotalCount++;
        }
    }
}
