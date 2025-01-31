namespace OnlineStore.Server.DTO.OrderElement
{
    public class OrderElementRequest
    {
        public Guid OrderId { get; set; }
        public Guid ItemId { get; set; }
        public required int ItemsCount { get; set; }
        public required double ItemPrice { get; set; }
    }
}
