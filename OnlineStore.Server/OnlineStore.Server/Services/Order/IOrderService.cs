using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Order;

namespace OnlineStore.Server.Services.Order
{
    public interface IOrderService
    {
        Task<Guid?> Create(OrderRequest order);
        Task<bool> Update(Guid id, OrderRequest order);
        Task<bool> Delete(Guid id);
        Task<bool> PlaceAnOrder(Guid orderId);
        Task<OrderResponse?> GetBasketOrder(Guid customerId);
        Task<ResponseList<OrderResponse>> GetPage(int pageNumber, int pageSize);
        Task<ResponseList<OrderResponse>> GetPageByCriteria(OrderFilterCriteria criteria, int pageNumber, int pageSize);
        Task<OrderResponse?> GetOneByCriteria(OrderFilterCriteria criteria);
    }
}
