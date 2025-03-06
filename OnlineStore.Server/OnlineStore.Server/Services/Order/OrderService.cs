using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Order;
using OnlineStore.Server.Mapping.Order;
using OnlineStore.Server.Repositories.Order;
using OnlineStore.Server.Validation.Customer;
using OnlineStore.Server.Validation.Order;

namespace OnlineStore.Server.Services.Order
{
    public class OrderService(IOrderRepository orderRepository) : IOrderService
    {
        private readonly IOrderRepository _orderRepository = orderRepository;

        public async Task<Guid?> CreateOrder(OrderRequest order)
        {
            bool isValid = OrderValidator.CheckRequest(order);

            if (isValid)
            {
                return await _orderRepository.CreateOrder(order);
            }
            
            return null;
        }

        public async Task<bool> UpdateOrder(Guid id, OrderRequest order)
        {
            bool isValid = OrderValidator.CheckGuid(id)
                        && OrderValidator.CheckRequest(order);

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

        public async Task<OrderResponse?> GetBasketOrder(Guid customerId)
        {
            if (!CustomerValidator.CheckGuid(customerId)) return null;

            if (await _orderRepository.GetOneByCriteria(new() { CustomerId = customerId }) is null) return null;

            OrderResponse? basket = await _orderRepository.GetOneByCriteria(new() { CustomerId = customerId, OrderStatus = "basket" });

            if (basket is null)
            {
                var request = new OrderRequest()
                {
                    CustomerId = customerId,
                    OrderDate = DateOnly.FromDateTime(DateTime.Now).ToString(),
                    OrderStatus = "basket"
                };

                Guid? guid = await _orderRepository.CreateOrder(request);

                basket = await _orderRepository.GetOneByCriteria(new() { Id = guid });
            }

            return basket;
        }

        public async Task<bool> PlaceAnOrder(Guid orderId)
        {
            if (!OrderValidator.CheckGuid(orderId)) return false;

            OrderResponse? basket = await _orderRepository.GetOneByCriteria(new() { Id = orderId, OrderStatus = "basket" });

            if (basket is null) return false;

            OrderRequest updateRequest = basket.MapToRequest();
            updateRequest.OrderStatus = "new";

            return await _orderRepository.UpdateOrder(basket.Id, updateRequest);
        }

        public async Task<ResponseList<OrderResponse>> GetPageOfOrders(int pageNumber, int pageSize)
        {
            bool isValid = OrderValidator.CheckPages(pageNumber, pageSize);

            if (isValid)
            {
                ResponseList<OrderResponse> response = await _orderRepository.GetAllOrders();

                response.Responses = response.Responses.Skip((pageNumber - 1) * pageSize)
                                                       .Take(pageSize)
                                                       .ToList();

                return response;
            }

            return new ResponseList<OrderResponse>();
        }

        public async Task<ResponseList<OrderResponse>> GetPageOfOrdersByCriteria(OrderFilterCriteria criteria, int pageNumber, int pageSize)
        {
            bool isValid = OrderValidator.CheckCriteria(criteria)
                        && OrderValidator.CheckPages(pageNumber, pageSize);

            if (isValid)
            {
                ResponseList<OrderResponse> response = await _orderRepository.GetOrdersByCriteria(criteria);

                response.Responses = response.Responses.Skip((pageNumber - 1) * pageSize)
                                                       .Take(pageSize)
                                                       .ToList();

                return response;
            }

            return new ResponseList<OrderResponse>();
        }

        public async Task<OrderResponse?> GetOneByCriteria(OrderFilterCriteria criteria)
        {
            bool isValid = OrderValidator.CheckCriteria(criteria);

            if (isValid)
            {
                return await _orderRepository.GetOneByCriteria(criteria);
            }

            return null;
        }
    }
}
