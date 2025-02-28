using OnlineStore.Server.Utilities.Order.Generators;

namespace OnlineStore.Server.Tests.Repositories.Order
{
    public class OrderGeneratorMock : INumberGenerator
    {
        private static int _maxNumber = 0;
        private readonly object _lock = new();

        public int GenerateNewNumber
        {
            get
            {
                lock (_lock)
                {
                    return ++_maxNumber;
                }
            }
        }
    }
}
