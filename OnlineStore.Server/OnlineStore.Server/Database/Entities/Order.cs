namespace OnlineStore.Server.Database.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
        public DateOnly OrderDate { get; set; }
        public DateOnly? ShipmentDate { get; set; }
        public int? OrderNumber { get; set; }
        public string? OrderStatus { get; set; }
        public virtual ICollection<OrderElement> OrderElements { get; set; } = [];

        public Order() { }
        public Order(Guid customerId, int orderNumber)
        {
            Id = Guid.NewGuid();
            CustomerId = customerId;
            OrderDate = DateOnly.FromDateTime(DateTime.Now);
            OrderNumber = orderNumber;
            OrderStatus = "new";
        }
    }
}
