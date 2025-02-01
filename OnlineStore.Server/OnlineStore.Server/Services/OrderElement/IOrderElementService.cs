using OnlineStore.Server.DTO.OrderElement;

namespace OnlineStore.Server.Services.OrderElement
{
    public interface IOrderElementService
    {
        Task<IEnumerable<OrderElementResponse>> GetOrderElementsByOrderId(Guid id);
        Task<Guid?> CreateOrderElement(OrderElementRequest orderElement);
        Task<bool> UpdateOrderElement(Guid id, OrderElementRequest orderElement);
        Task<bool> DeleteOrderElement(Guid id);
        Task<IEnumerable<OrderElementResponse>> GetBasketByCustomerId(Guid id);
    }
}
