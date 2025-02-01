﻿using OnlineStore.Server.DTO.Order;

namespace OnlineStore.Server.Repositories.Order
{
    public interface IOrderRepository
    {
        Task<OrderResponseList> GetPageOfOrders(int pageNumber, int pageSize); //add count() to response
        Task<OrderResponseList> GetPageOfOrdersByCustomerId(Guid id, int pageNumber, int pageSize); //add count() to response
        Task<OrderResponseList> GetPageOfOrdersByStatus(string status, int pageNumber, int pageSize); //add count() to response
        Task<OrderResponse?> GetOrderByNumber(int number);
        Task<Guid?> CreateOrder(OrderRequest order);
        Task<bool> UpdateOrder(Guid id, OrderRequest order);
        Task<bool> DeleteOrder(Guid id);
    }
}
