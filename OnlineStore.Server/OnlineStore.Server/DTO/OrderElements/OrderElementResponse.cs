using OnlineStore.Server.DTO.Items;

namespace OnlineStore.Server.DTO.OrderElements
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
