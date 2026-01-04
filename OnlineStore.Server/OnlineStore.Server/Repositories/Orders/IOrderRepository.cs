using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Orders;

namespace OnlineStore.Server.Repositories.Orders
{
    public interface IOrderRepository
    {
        Task<Guid?> Create(OrderRequest order);
        Task<bool> Update(Guid orderId, OrderRequest order);
        Task<bool> Delete(Guid orderId);
        Task<ResponseList<OrderResponse>> GetAll();
        Task<ResponseList<OrderResponse>> GetAllByCriteria(OrderFilterCriteria criteria);
        Task<OrderResponse?> GetOneByCriteria(OrderFilterCriteria criteria);
    }
}
