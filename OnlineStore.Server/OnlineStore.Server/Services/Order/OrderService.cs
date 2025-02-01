using OnlineStore.Server.DTO.Order;
using OnlineStore.Server.Repositories.Order;
using OnlineStore.Server.Validation.Order;

namespace OnlineStore.Server.Services.Order
{
    public class OrderService(IOrderRepository orderRepository) : IOrderService
    {
        private readonly IOrderRepository _orderRepository = orderRepository;

        public async Task<Guid?> CreateOrder(OrderRequest order)
        {
            bool isValid = OrderValidator.CheckDates(order.OrderDate, order.ShipmentDate);

            if (isValid)
            {
                return await _orderRepository.CreateOrder(order);
            }
            
            return null;
        }

        public async Task<bool> UpdateOrder(Guid id, OrderRequest order)
        {
            bool isValid = OrderValidator.CheckGuid(id)
                        && OrderValidator.CheckDates(order.OrderDate, order.ShipmentDate);

            if (isValid)
            {
                return await _orderRepository.UpdateOrder(id, order);
            }

            return false;
        }

        public async Task<bool> DeleteOrder(Guid id)
        {
            bool isValid = OrderValidator.CheckGuid(id);

            if (isValid)
            {
                return await _orderRepository.DeleteOrder(id);
            }

            return false;
        }

        public async Task<OrderResponse?> GetOrderByNumber(int number)
        {
            return await _orderRepository.GetOrderByNumber(number);
        }

        public async Task<OrderResponseList> GetPageOfOrders(int pageNumber, int pageSize)
        {
            bool isValid = OrderValidator.CheckPages(pageNumber, pageSize);

            if (isValid)
            {
                return await _orderRepository.GetPageOfOrders(pageNumber, pageSize);
            }

            return new OrderResponseList();
        }

        public async Task<OrderResponseList> GetPageOfOrdersByCustomerId(Guid id, int pageNumber, int pageSize)
        {
            bool isValid = OrderValidator.CheckPages(pageNumber, pageSize)
                        && OrderValidator.CheckGuid(id);

            if (isValid)
            {
                return await _orderRepository.GetPageOfOrdersByCustomerId(id, pageNumber, pageSize);
            }
            
            return new OrderResponseList();
        }

        public async Task<OrderResponseList> GetPageOfOrdersByStatus(string status, int pageNumber, int pageSize)
        {
            bool isValid = OrderValidator.CheckPages(pageNumber, pageSize);

            if (isValid)
            {
                return await _orderRepository.GetPageOfOrdersByStatus(status, pageNumber, pageSize);
            }

            return new OrderResponseList();
        }

        
    }
}
