using OnlineStore.Server.DTO.OrderElements;
using OnlineStore.Server.Repositories.OrderElements;
using OnlineStore.Server.Validation.OrderElements;

namespace OnlineStore.Server.Services.OrderElements
{
    public class OrderElementService(IOrderElementRepository orderElementRepository) : IOrderElementService
    {
        private readonly IOrderElementRepository _orderElementRepository = orderElementRepository;
        
        public async Task<Guid?> Create(OrderElementRequest request)
        {
            bool isValid = OrderElementValidator.CheckGuid(request.OrderId)
                        && OrderElementValidator.CheckGuid(request.ItemId)
                        && OrderElementValidator.CheckCount(request.ItemsCount)
                        && OrderElementValidator.CheckPrice(request.ItemPrice);

            if (isValid)
            {
                return await _orderElementRepository.Create(request);
            }

            return null;
        }

        public async Task<bool> Update(Guid id, UpdateOrderElementRequest request)
        {
            bool isValid = OrderElementValidator.CheckGuid(id)
                        && OrderElementValidator.CheckCount(request.ItemsCount)
                        && OrderElementValidator.CheckPrice(request.ItemPrice);

            if (isValid)
            {
                return await _orderElementRepository.Update(id, request);
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
