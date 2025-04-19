using OnlineStore.Server.DTO.OrderElement;
using OnlineStore.Server.Repositories.OrderElement;
using OnlineStore.Server.Validation.OrderElement;

namespace OnlineStore.Server.Services.OrderElement
{
    public class OrderElementService(IOrderElementRepository orderElementRepository) : IOrderElementService
    {
        private readonly IOrderElementRepository _orderElementRepository = orderElementRepository;
        
        public async Task<Guid?> Create(OrderElementRequest orderElement)
        {
            bool isValid = OrderElementValidator.CheckGuid(orderElement.OrderId)
                        && OrderElementValidator.CheckGuid(orderElement.ItemId)
                        && OrderElementValidator.CheckCount(orderElement.ItemsCount)
                        && OrderElementValidator.CheckPrice(orderElement.ItemPrice);

            if (isValid)
            {
                return await _orderElementRepository.Create(orderElement);
            }

            return null;
        }

        public async Task<bool> Update(Guid id, UpdateOrderElementRequest orderElement)
        {
            bool isValid = OrderElementValidator.CheckGuid(id)
                        && OrderElementValidator.CheckCount(orderElement.ItemsCount)
                        && OrderElementValidator.CheckPrice(orderElement.ItemPrice);

            if (isValid)
            {
                return await _orderElementRepository.Update(id, orderElement);
            }

            return false;
        }

        public async Task<bool> Delete(Guid id)
        {
            bool isValid = OrderElementValidator.CheckGuid(id);

            if (isValid)
            {
                return await _orderElementRepository.Delete(id);
            }

            return false;
        }

        public async Task<IEnumerable<OrderElementResponse>> GetAllByOrderId(Guid id)
        {
            bool isValid = OrderElementValidator.CheckGuid(id);

            if (isValid)
            {
                return await _orderElementRepository.GetAllByOrderId(id);
            }

            return [];
        }
    }
}
