using OnlineStore.Server.DTO.OrderElement;

namespace OnlineStore.Server.Services.OrderElement
{
    public interface IOrderElementService
    {
        Task<Guid?> Create(OrderElementRequest request);
        Task<bool> Update(Guid id, UpdateOrderElementRequest request);
        Task<bool> Delete(Guid id);
        Task<IEnumerable<OrderElementResponse>> GetAllByOrderId(Guid id);
    }
}
