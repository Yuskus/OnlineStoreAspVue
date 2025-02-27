using OnlineStore.Server.Tests.Common;

namespace OnlineStore.Server.Tests.Repositories.OrderElement
{
    [Collection("DatabaseCollection")]
    public class OrderElementRepositoryTest : IClassFixture<OrderElementDbContextFixture>
    {
        /*Task<IEnumerable<OrderElementResponse>> GetOrderElementsByOrderId(Guid id);
        Task<Guid?> CreateOrderElement(OrderElementRequest orderElement);
        Task<bool> UpdateOrderElement(Guid id, OrderElementRequest orderElement);
        Task<bool> DeleteOrderElement(Guid id);*/

        private readonly FakeDbContext _context;

        public OrderElementRepositoryTest(OrderElementDbContextFixture fixture)
        {
            _context = fixture.Context;
        }
    }
}
