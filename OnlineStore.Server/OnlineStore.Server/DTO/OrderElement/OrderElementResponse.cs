using OnlineStore.Server.DTO.Item;

namespace OnlineStore.Server.DTO.OrderElement
{
    public class OrderElementResponse
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Guid ItemId { get; set; }
        public ItemResponse? ItemResponse { get; set; }
        public required int ItemsCount { get; set; }
        public required double ItemPrice { get; set; }
    }
}
