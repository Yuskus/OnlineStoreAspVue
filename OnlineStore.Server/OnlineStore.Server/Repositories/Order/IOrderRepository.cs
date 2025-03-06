using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Order;

namespace OnlineStore.Server.Repositories.Order
{
    public interface IOrderRepository
    {
        Task<ResponseList<OrderResponse>> GetAllOrders();
        Task<ResponseList<OrderResponse>> GetOrdersByCriteria(OrderFilterCriteria criteria);
        Task<OrderResponse?> GetOneByCriteria(OrderFilterCriteria criteria);
        Task<Guid?> CreateOrder(OrderRequest order);
        Task<bool> UpdateOrder(Guid id, OrderRequest order);
        Task<bool> DeleteOrder(Guid id);
    }
}
