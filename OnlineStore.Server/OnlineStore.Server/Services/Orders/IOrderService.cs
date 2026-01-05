using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Orders;

namespace OnlineStore.Server.Services.Orders
{
    public interface IOrderService
    {
        Task<Guid?> Create(OrderRequest request);
        Task<bool> Update(Guid id, OrderRequest request);
        Task<bool> Delete(Guid id);
        Task<bool> PlaceAnOrder(Guid id);
        Task<OrderResponse?> GetBasketOrder(Guid customerId);
        Task<ResponseList<OrderResponse>> GetPage(PageInfo pageInfo);
        Task<ResponseList<OrderResponse>> GetPageByCriteria(OrderFilterCriteria criteria, PageInfo pageInfo);
    }
}
