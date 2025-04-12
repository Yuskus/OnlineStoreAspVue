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

        public async Task<Guid?> Create(OrderRequest order)
        {
            bool isValid = OrderValidator.CheckRequest(order);

            if (isValid)
            {
                return await _orderRepository.Create(order);
            }
            
            return null;
        }

        public async Task<bool> Update(Guid orderId, OrderRequest order)
        {
            bool isValid = OrderValidator.CheckGuid(orderId)
                        && OrderValidator.CheckRequest(order);

            if (isValid)
            {
                return await _orderRepository.Update(orderId, order);
            }

            return false;
        }

        public async Task<bool> Delete(Guid orderId)
        {
            bool isValid = OrderValidator.CheckGuid(orderId);

            if (isValid)
            {
                return await _orderRepository.Delete(orderId);
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

                Guid? guid = await _orderRepository.Create(request);

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

            return await _orderRepository.Update(basket.Id, updateRequest);
        }

        public async Task<ResponseList<OrderResponse>> GetPage(int pageNumber, int pageSize)
        {
            bool isValid = OrderValidator.CheckPages(pageNumber, pageSize);

            if (isValid)
            {
                ResponseList<OrderResponse> response = await _orderRepository.GetAll();

                response.Responses = response.Responses.Skip((pageNumber - 1) * pageSize)
                                                       .Take(pageSize)
                                                       .ToList();

                return response;
            }

            return new ResponseList<OrderResponse>();
        }

        public async Task<ResponseList<OrderResponse>> GetPageByCriteria(OrderFilterCriteria criteria, int pageNumber, int pageSize)
        {
            bool isValid = OrderValidator.CheckCriteria(criteria)
                        && OrderValidator.CheckPages(pageNumber, pageSize);

            if (isValid)
            {
                ResponseList<OrderResponse> response = await _orderRepository.GetAllByCriteria(criteria);

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
