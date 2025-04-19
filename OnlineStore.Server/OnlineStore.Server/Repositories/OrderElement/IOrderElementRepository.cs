using OnlineStore.Server.DTO.OrderElement;

namespace OnlineStore.Server.Repositories.OrderElement
{
    public interface IOrderElementRepository
    {
        Task<Guid?> Create(OrderElementRequest orderElement);
        Task<bool> Update(Guid id, UpdateOrderElementRequest orderElement);
        Task<bool> Delete(Guid id);
        Task<IEnumerable<OrderElementResponse>> GetAllByOrderId(Guid id);
    }
}
