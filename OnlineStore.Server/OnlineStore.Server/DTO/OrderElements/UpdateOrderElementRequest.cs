namespace OnlineStore.Server.DTO.OrderElements
{
    public class UpdateOrderElementRequest
    {
        public required int ItemsCount { get; set; }
        public required double ItemPrice { get; set; }
    }
}
