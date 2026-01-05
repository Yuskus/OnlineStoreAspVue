namespace OnlineStore.Server.DTO.Orders
{
    public class OrderFilterCriteria
    {
        public Guid? Id { get; set; } //one
        public Guid? CustomerId { get; set; }
        public int? OrderNumber { get; set; } //one
        public string? OrderStatus { get; set; }
    }
}
