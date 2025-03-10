namespace OnlineStore.Server.Tests.Services.OrderElement
{
    [Collection("OrderElementServiceCollection")]
    public class OrderElementServiceTest : IClassFixture<OrderElementServiceFixture>
    {
        /*Task<Guid?> Create(OrderElementRequest orderElement);
        Task<bool> Update(Guid id, OrderElementRequest orderElement);
        Task<bool> Delete(Guid id);
        Task<IEnumerable<OrderElementResponse>> GetAllByOrderId(Guid id);*/

        private readonly OrderElementServiceFixture _fixture;
        public OrderElementServiceTest(OrderElementServiceFixture fixture)
        {
            _fixture = fixture;
        }
    }
}
