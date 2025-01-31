using OnlineStore.Server.DTO.OrderElement;
using OnlineStore.Server.Repositories.OrderElement;
using OnlineStore.Server.Validation.OrderElement;

namespace OnlineStore.Server.Services.OrderElement
{
    public class OrderElementService(IOrderElementRepository orderElementRepository) : IOrderElementService
    {
        private readonly IOrderElementRepository _orderElementRepository = orderElementRepository;
        
        public async Task<Guid?> CreateOrderElement(OrderElementRequest orderElement)
        {
            bool isValid = OrderElementValidator.CheckGuid(orderElement.OrderId)
                        && OrderElementValidator.CheckGuid(orderElement.ItemId)
                        && OrderElementValidator.CheckCount(orderElement.ItemsCount)
                        && OrderElementValidator.CheckPrice(orderElement.ItemPrice);

            if (isValid)
            {
                return await _orderElementRepository.CreateOrderElement(orderElement);
            }

            return null;
        }

        public async Task<bool> UpdateOrderElement(Guid id, OrderElementRequest orderElement)
        {
            bool isValid = OrderElementValidator.CheckGuid(id)
                        && OrderElementValidator.CheckGuid(orderElement.OrderId)
                        && OrderElementValidator.CheckGuid(orderElement.ItemId)
                        && OrderElementValidator.CheckCount(orderElement.ItemsCount)
                        && OrderElementValidator.CheckPrice(orderElement.ItemPrice);

            if (isValid)
            {
                return await _orderElementRepository.UpdateOrderElement(id, orderElement);
            }

            return false;
        }

        public async Task<bool> DeleteOrderElement(Guid id)
        {
            bool isValid = OrderElementValidator.CheckGuid(id);

            if (isValid)
            {
                return await _orderElementRepository.DeleteOrderElement(id);
            }

            return false;
        }

        public async Task<IEnumerable<OrderElementResponse>> GetOrderElementsByOrderId(Guid id)
        {
            bool isValid = OrderElementValidator.CheckGuid(id);

            if (isValid)
            {
                return await _orderElementRepository.GetOrderElementsByOrderId(id);
            }

            return [];
        }

        public async Task<IEnumerable<OrderElementResponse>> GetBasketByCustomerId(Guid id)
        {
            bool isValid = OrderElementValidator.CheckGuid(id);

            if (isValid)
            {
                return await _orderElementRepository.GetBasketByCustomerId(id);
            }

            return [];
        }
    }
}
