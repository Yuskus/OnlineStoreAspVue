﻿using OnlineStore.Server.DTO.OrderElement;

namespace OnlineStore.Server.Repositories.OrderElement
{
    public interface IOrderElementRepository
    {
        Task<IEnumerable<OrderElementResponse>> GetOrderElementsByOrderId(Guid id); // is it needed?
        Task<Guid?> CreateOrderElement(OrderElementRequest orderElement);
        Task<bool> UpdateOrderElement(Guid id, OrderElementRequest orderElement);
        Task<bool> DeleteOrderElement(Guid id);
        Task<IEnumerable<OrderElementResponse>> GetBasketByCustomerId(Guid id);
    }
}
