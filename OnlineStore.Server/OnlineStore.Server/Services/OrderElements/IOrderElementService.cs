using OnlineStore.Server.DTO.OrderElements;

namespace OnlineStore.Server.Services.OrderElements
{
    public interface IOrderElementService
    {
        Task<Guid?> Create(OrderElementRequest request);
        Task<bool> Update(Guid id, UpdateOrderElementRequest request);
        Task<bool> Delete(Guid id);
        Task<IEnumerable<OrderElementResponse>> GetAllByOrderId(Guid id);
    }
}
