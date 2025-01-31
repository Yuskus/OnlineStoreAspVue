namespace OnlineStore.Server.DTO.Order
{
    public class OrderRequest
    {
        public Guid CustomerId { get; set; }
        public required string OrderDate { get; set; }
        public string? ShipmentDate { get; set; }
        public int? OrderNumber { get; set; }
        public string? OrderStatus { get; set; }
    }
}
