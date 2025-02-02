using OnlineStore.Server.DTO.Order;

namespace OnlineStore.Server.Services.Order
{
    public interface IOrderService
    {
        Task<OrderResponseList> GetPageOfOrders(int pageNumber, int pageSize);
        Task<OrderResponseList> GetPageOfOrdersByCustomerId(Guid id, int pageNumber, int pageSize);
        Task<OrderResponseList> GetPageOfOrdersByStatus(string status, int pageNumber, int pageSize);
        Task<OrderResponse?> GetOrderByNumber(int number);
        Task<OrderResponse?> GetBasketOrder(Guid customerId);
        Task<bool> PlaceAnOrder(Guid orderId);
        Task<Guid?> CreateOrder(OrderRequest order);
        Task<bool> UpdateOrder(Guid id, OrderRequest order);
        Task<bool> DeleteOrder(Guid id);
    }
}
