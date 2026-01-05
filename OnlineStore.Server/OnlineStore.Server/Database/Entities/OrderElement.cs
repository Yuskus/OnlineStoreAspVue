namespace OnlineStore.Server.Database.Entities
{
    public class OrderElement : BaseEntity
    {
        public Guid OrderId { get; set; }
        public virtual Order? Order { get; set; }
        public Guid ItemId { get; set; }
        public virtual Item? Item { get; set; }
        public required int ItemsCount { get; set; }
        public required double ItemPrice { get; set; }
    }
}
