using OnlineStore.Server.Constants.Orders;
using OnlineStore.Server.Database.Entities;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Orders;
using OnlineStore.Server.Mapping.Orders;
using OnlineStore.Server.Repositories.Orders;
using OnlineStore.Server.Validation.Customers;
using OnlineStore.Server.Validation.Orders;

namespace OnlineStore.Server.Services.Orders
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

            OrderResponse? basket = await _orderRepository.GetOneByCriteria(new()
            {
                CustomerId = customerId,
                OrderStatus = OrderStatuses.Basket
            });

            if (basket is null)
            {
                var request = new OrderRequest()
                {
                    CustomerId = customerId,
                    OrderDate = DateOnly.FromDateTime(DateTime.Now).ToString(),
                    OrderStatus = OrderStatuses.Basket
                };

                Guid? guid = await _orderRepository.Create(request);

                basket = await _orderRepository.GetOneByCriteria(new() { Id = guid });
            }

            return basket;
        }

        public async Task<bool> PlaceAnOrder(Guid id)
        {
            if (!OrderValidator.CheckGuid(id)) return false;

            OrderResponse? basket = await _orderRepository.GetOneByCriteria(new()
            {
                Id = id,
                OrderStatus = OrderStatuses.Basket
            });

            if (basket is null) return false;

            OrderRequest updateRequest = basket.MapToRequest();
            updateRequest.OrderStatus = OrderStatuses.New;

            return await _orderRepository.Update(basket.Id, updateRequest);
        }

        public async Task<ResponseList<OrderResponse>> GetPage(PageInfo pageInfo)
        {
            bool isValid = OrderValidator.CheckPages(pageInfo.Number, pageInfo.Size);

            if (isValid)
            {
                return await _orderRepository.GetPage(pageInfo);
            }

            return new ResponseList<OrderResponse>();
        }

        public async Task<ResponseList<OrderResponse>> GetPageByCriteria(OrderFilterCriteria criteria, PageInfo pageInfo)
        {
            bool isValid = OrderValidator.CheckCriteria(criteria)
                        && OrderValidator.CheckPages(pageInfo.Number, pageInfo.Size);

            if (isValid)
            {
                return await _orderRepository.GetPageByCriteria(criteria, pageInfo);
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
