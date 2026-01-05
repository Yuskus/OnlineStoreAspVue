using OnlineStore.Server.Constants.Orders;
using OnlineStore.Server.Database.Entities;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Customers;
using OnlineStore.Server.DTO.Orders;
using OnlineStore.Server.Repositories.Customers;
using OnlineStore.Server.Repositories.Orders;
using OnlineStore.Server.Validation.Customers;
using OnlineStore.Server.Validation.Orders;

namespace OnlineStore.Server.Services.Orders
{
    public class OrderService(
        IOrderRepository orderRepository,
        ICustomerRepository customerRepository) : IOrderService
    {
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly ICustomerRepository _customerRepository = customerRepository;

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

            if (await _customerRepository.Get(customerId) is null) return null;

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
                    OrderDate = DateOnly.FromDateTime(DateTime.Now).ToString("yyyy-MM-dd"),
                    OrderStatus = OrderStatuses.Basket
                };

                Guid? guid = await _orderRepository.Create(request);

                if (guid != null)
                {
                    basket = await _orderRepository.GetOneByCriteria(new() { Id = guid });
                }
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

            return await _orderRepository.Update(basket.Id, new()
            {
                CustomerId = basket.CustomerId,
                OrderDate = basket.OrderDate.ToString(),
                ShipmentDate = basket.ShipmentDate?.ToString(),
                OrderStatus = OrderStatuses.New
            });
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
    }
}
