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

        public async Task<Guid?> Create(OrderRequest request)
        {
            bool isValid = OrderValidator.CheckRequest(request);

            if (isValid)
            {
                return await _orderRepository.Create(request);
            }
            
            return null;
        }

        public async Task<bool> Update(Guid id, OrderRequest request)
        {
            bool isValid = OrderValidator.CheckGuid(id)
                        && OrderValidator.CheckRequest(request);

            if (isValid)
            {
                return await _orderRepository.Update(id, request);
            }

            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            bool isValid = OrderValidator.CheckGuid(id);

            if (isValid)
            {
                return await _orderRepository.Delete(id);
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

        public async Task<bool> PlaceAnOrder(Guid id)
        {
            if (!OrderValidator.CheckGuid(id)) return false;

            OrderResponse? basket = await _orderRepository.GetOneByCriteria(new() { Id = id, OrderStatus = "basket" });

            if (basket is null) return false;

            OrderRequest updateRequest = basket.MapToRequest();
            updateRequest.OrderStatus = "new";

            return await _orderRepository.Update(basket.Id, updateRequest);
        }

        public async Task<ResponseList<OrderResponse>> GetPage(int page, int pageSize)
        {
            bool isValid = OrderValidator.CheckPages(page, pageSize);

            if (isValid)
            {
                ResponseList<OrderResponse> response = await _orderRepository.GetAll();

                response.Responses = [.. response.Responses.Skip((page - 1) * pageSize).Take(pageSize)];

                return response;
            }

            return new ResponseList<OrderResponse>();
        }

        public async Task<ResponseList<OrderResponse>> GetPageByCriteria(OrderFilterCriteria criteria, int page, int pageSize)
        {
            bool isValid = OrderValidator.CheckCriteria(criteria)
                        && OrderValidator.CheckPages(page, pageSize);

            if (isValid)
            {
                ResponseList<OrderResponse> response = await _orderRepository.GetAllByCriteria(criteria);

                response.Responses = [.. response.Responses.Skip((page - 1) * pageSize).Take(pageSize)];

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
