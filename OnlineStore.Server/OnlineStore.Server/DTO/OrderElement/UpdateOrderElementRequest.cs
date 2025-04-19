namespace OnlineStore.Server.DTO.OrderElement
{
    public class UpdateOrderElementRequest
    {
        public required int ItemsCount { get; set; }
        public required double ItemPrice { get; set; }
    }
}
