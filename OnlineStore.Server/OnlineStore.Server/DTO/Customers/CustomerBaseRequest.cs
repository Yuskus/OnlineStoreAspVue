namespace OnlineStore.Server.DTO.Customers
{
    public class CustomerBaseRequest
    {
        public required string Name { get; set; }
        public required string Code { get; set; }
        public string? Address { get; set; }
    }
}
