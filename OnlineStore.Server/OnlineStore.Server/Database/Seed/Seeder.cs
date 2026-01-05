using OnlineStore.Server.Authorization.Utilities;
using OnlineStore.Server.Database.Context;
using OnlineStore.Server.Database.Entities;

namespace OnlineStore.Server.Database.Seed
{
    public static class Seeder
    {
        public static async Task StartSeed(OnlineStoreDbContext dbContext)
        {
            try
            {
                if (dbContext.Customers.Any() &&
                    dbContext.Users.Any() &&
                    dbContext.Items.Any())
                    return;

                var customerId = Guid.NewGuid();

                var (hash1, salt1) = Hasher.CreatePasswordHash("12345678");
                var (hash2, salt2) = Hasher.CreatePasswordHash("1234567890");

                // customer
                dbContext.Customers.Add(new Customer
                {
                    Id = customerId,
                    Name = "Заказчиков Заказчик Заказчикович",
                    Code = "0000-2000",
                    Address = "г. Заказчиков, ул. Заказная, д.1, кв.1",
                    Discount = 5
                });
                dbContext.Users.Add(new User
                {
                    CustomerId = customerId,
                    Username = "user@mail.ru",
                    Password = hash1,
                    Salt = salt1,
                    Role = UserRole.User
                });

                // manager
                dbContext.Users.Add(new User
                {
                    Username = "admin@mail.ru",
                    Password = hash2,
                    Salt = salt2,
                    Role = UserRole.Manager
                });

                // items
                dbContext.Items.AddRange(new List<Item>
                {
                    new()
                    {
                        Code = "00-0000-AA00",
                        Name = "Яблоко",
                        Price = Random.Shared.Next(15, 100),
                        Category = "Фрукты"
                    },
                    new()
                    {
                        Code = "00-0000-AA01",
                        Name = "Огурец",
                        Price = Random.Shared.Next(15, 100),
                        Category = "Овощи"
                    },
                    new()
                    {
                        Code = "00-0000-AA02",
                        Name = "Помидор",
                        Price = Random.Shared.Next(15, 100),
                        Category = "Овощи"
                    },
                    new()
                    {
                        Code = "00-0000-AA03",
                        Name = "Апельсин",
                        Price = Random.Shared.Next(15, 100),
                        Category = "Фрукты"
                    },
                    new()
                    {
                        Code = "00-0000-AA04",
                        Name = "Грейпфрут",
                        Price = Random.Shared.Next(15, 100),
                        Category = "Фрукты"
                    },
                    new()
                    {
                        Code = "00-0000-AA05",
                        Name = "Арбуз",
                        Price = Random.Shared.Next(15, 100),
                        Category = "Ягоды"
                    },
                    new()
                    {
                        Code = "00-0000-AA06",
                        Name = "Картофель",
                        Price = Random.Shared.Next(15, 100),
                        Category = "Овощи"
                    },
                    new()
                    {
                        Code = "00-0000-AA07",
                        Name = "Лук",
                        Price = Random.Shared.Next(15, 100),
                        Category = "Овощи"
                    },
                    new()
                    {
                        Code = "00-0000-AA08",
                        Name = "Чеснок",
                        Price = Random.Shared.Next(15, 100),
                        Category = "Овощи"
                    },
                    new()
                    {
                        Code = "00-0000-AA09",
                        Name = "Мандарин",
                        Price = Random.Shared.Next(15, 100),
                        Category = "Фрукты"
                    },
                    new()
                    {
                        Code = "00-0000-AA10",
                        Name = "Маслины",
                        Price = Random.Shared.Next(15, 100),
                        Category = "Овощи"
                    },
                    new()
                    {
                        Code = "00-0000-AA11",
                        Name = "Икра",
                        Price = Random.Shared.Next(15, 100),
                        Category = "Рыба"
                    },
                    new()
                    {
                        Code = "00-0000-AA12",
                        Name = "Сёмга",
                        Price = Random.Shared.Next(15, 100),
                        Category = "Рыба"
                    },
                    new()
                    {
                        Code = "00-0000-AA13",
                        Name = "Масло",
                        Price = Random.Shared.Next(15, 100),
                        Category = "Молочка"
                    },
                    new()
                    {
                        Code = "00-0000-AA14",
                        Name = "Чипсы",
                        Price = Random.Shared.Next(15, 100),
                        Category = "Бакалея"
                    },
                    new()
                    {
                        Code = "00-0000-AA15",
                        Name = "Сухарики",
                        Price = Random.Shared.Next(15, 100),
                        Category = "Бакалея"
                    },
                    new()
                    {
                        Code = "00-0000-AA16",
                        Name = "Телятина",
                        Price = Random.Shared.Next(15, 100),
                        Category = "Мясо"
                    },
                    new()
                    {
                        Code = "00-0000-AA17",
                        Name = "Ряженка",
                        Price = Random.Shared.Next(15, 100),
                        Category = "Кисломолочное"
                    },
                    new()
                    {
                        Code = "00-0000-AA18",
                        Name = "Кефир",
                        Price = Random.Shared.Next(15, 100),
                        Category = "Кисломолочное"
                    },
                    new()
                    {
                        Code = "00-0000-AA19",
                        Name = "Молоко",
                        Price = Random.Shared.Next(15, 100),
                        Category = "Кисломолочное"
                    },
                    new()
                    {
                        Code = "00-0000-AA20",
                        Name = "Холодец",
                        Price = Random.Shared.Next(15, 100),
                        Category = "Мясо"
                    }
                });

                await dbContext.SaveChangesAsync();
            }
            catch
            {
                Console.WriteLine("Seed failed.");
            }
        }
    }
}
