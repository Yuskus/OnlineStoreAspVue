using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Order;

namespace OnlineStore.Server.Repositories.Order
{
    public interface IOrderRepository
    {
        Task<ResponseList<OrderResponse>> GetPageOfOrders(int pageNumber, int pageSize);
        Task<ResponseList<OrderResponse>> GetPageOfOrdersByCustomerId(Guid id, int pageNumber, int pageSize);
        Task<ResponseList<OrderResponse>> GetPageOfOrdersByStatus(string status, int pageNumber, int pageSize);
        Task<OrderResponse?> GetOrderByNumber(int number);
        Task<OrderResponse?> GetBasketOrder(Guid customerId);
        Task<bool> PlaceAnOrder(Guid orderId);
        Task<Guid?> CreateOrder(OrderRequest order);
        Task<bool> UpdateOrder(Guid id, OrderRequest order);
        Task<bool> DeleteOrder(Guid id);
    }
}
