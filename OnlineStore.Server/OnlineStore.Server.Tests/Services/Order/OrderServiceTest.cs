namespace OnlineStore.Server.Tests.Services.Order
{
    [Collection("OrderServiceCollection")]
    public class OrderServiceTest : IClassFixture<OrderServiceFixture>
    {
        /*Task<Guid?> Create(OrderRequest order);
        Task<bool> Update(Guid id, OrderRequest order);
        Task<bool> Delete(Guid id);
        Task<bool> PlaceAnOrder(Guid orderId);
        Task<OrderResponse?> GetBasketOrder(Guid customerId);
        Task<ResponseList<OrderResponse>> GetPage(int pageNumber, int pageSize);
        Task<ResponseList<OrderResponse>> GetPageByCriteria(OrderFilterCriteria criteria, int pageNumber, int pageSize);
        Task<OrderResponse?> GetOneByCriteria(OrderFilterCriteria criteria);*/

        private readonly OrderServiceFixture _fixture;
        public OrderServiceTest(OrderServiceFixture fixture)
        {
            _fixture = fixture;
        }
    }
}
