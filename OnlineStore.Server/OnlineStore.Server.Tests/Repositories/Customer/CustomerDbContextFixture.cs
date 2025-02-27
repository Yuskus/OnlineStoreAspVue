using OnlineStore.Server.Tests.Common;
using Entity = OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Tests.Repositories.Customer
{
    public class CustomerDbContextFixture : DbContextFixture
    {
        public Guid CustomerId_ForGetById { get; private set; }
        public Guid CustomerId_ForUpdate { get; private set; }
        public Guid CustomerId_Unexist { get; private set; } = Guid.NewGuid();
        public string CustomerCode_ForGetByCode { get; private set; } = string.Empty;
        public string CustomerCode_Unexists { get; private set; } = string.Empty;
        public int CustomersTotalCount { get; private set; }

        public CustomerDbContextFixture()
        {
            Initialize();
        }

        private void Initialize(int capacity = 30)
        {
            // заполнение БД (customers)

            var customers = new Entity.Customer[capacity];

            static string getCode(int i) => $"{1000 + i}-2000";

            for (int i = 0; i < capacity; i++)
            {
                customers[i] = new()
                {
                    Name = $"TestName{i}",
                    Code = getCode(i),
                    Discount = i
                };
            }

            Context.Customers.AddRange(customers);
            Context.SaveChanges();

            // инициализация переменных для тестов

            CustomerCode_ForGetByCode = getCode(capacity / 2); // некоторый код от пользователя из середины
            CustomerCode_Unexists = "9999-2000"; // явно несуществующий код

            CustomerId_ForGetById = Context.Customers.First().Id; // некоторый случайный Id (номер в discount будет "0")
            CustomerId_ForUpdate = Context.Customers.Last().Id; // некоторый случайный Id (номер п/п в discount будет "capacity - 1")

            CustomersTotalCount = capacity;
        }
    }
}
