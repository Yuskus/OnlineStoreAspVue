namespace OnlineStore.Server.DTO.Items
{
    public class ItemRequest
    {
        public required string Code { get; set; }
        public required string Name { get; set; }
        public double? Price { get; set; }
        public string? Category { get; set; }
    }
}
