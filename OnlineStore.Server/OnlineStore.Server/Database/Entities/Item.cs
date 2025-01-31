namespace OnlineStore.Server.Database.Entities
{
    public class Item
    {
        public Guid Id { get; set; }
        public required string Code { get; set; }
        public required string Name { get; set; }
        public double? Price { get; set; }
        public string? Category { get; set; }
        public virtual ICollection<OrderElement> OrderElements { get; set; } = [];
    }
}
