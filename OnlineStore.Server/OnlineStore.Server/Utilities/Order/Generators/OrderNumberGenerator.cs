using OnlineStore.Server.Database.Context;

namespace OnlineStore.Server.Utilities.Order.Generators
{
    public class OrderNumberGenerator : INumberGenerator
    {
        private int _maxNumber = -1;
        private readonly object _lock = new();
        private readonly OnlineStoreDbContext _context;
        private readonly ILogger<OrderNumberGenerator> _logger;

        public OrderNumberGenerator(OnlineStoreDbContext context, ILogger<OrderNumberGenerator> logger)
        {
            _context = context;
            _logger = logger;
            Initialize();
        }

        private void Initialize()
        {
            try
            {
                _maxNumber = _context.Orders.Max(x => x.OrderNumber) ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при инициализации _maxNumber в классе OrderNumberGenerator " +
                                     "(вероятно, проблема с доступом к базе данных).");
                _maxNumber = -1;
                throw;
            }
        }

        public int GenerateNewNumber
        {
            get
            {
                lock (_lock)
                {
                    if (_maxNumber < 0)
                    {
                        Initialize();
                    }

                    return ++_maxNumber;
                }
            }
        }
    }
}
