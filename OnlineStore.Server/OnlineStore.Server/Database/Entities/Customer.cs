namespace OnlineStore.Server.Database.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Code { get; set; }
        public string? Address { get; set; }
        public int Discount { get; set; } = 0;
        public virtual ICollection<Order> Orders { get; set; } = [];
        public virtual User? User { get; set; }
    }
}
