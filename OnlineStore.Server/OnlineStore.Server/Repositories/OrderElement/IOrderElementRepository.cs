﻿using OnlineStore.Server.DTO.OrderElement;

namespace OnlineStore.Server.Repositories.OrderElement
{
    public interface IOrderElementRepository
    {
        Task<IEnumerable<OrderElementResponse>> GetOrderElementsByOrderId(Guid id);
        Task<Guid?> CreateOrderElement(OrderElementRequest orderElement);
        Task<bool> UpdateOrderElement(Guid id, OrderElementRequest orderElement);
        Task<bool> DeleteOrderElement(Guid id);
    }
}
