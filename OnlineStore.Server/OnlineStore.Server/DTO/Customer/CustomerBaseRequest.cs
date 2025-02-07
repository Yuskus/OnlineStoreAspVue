namespace OnlineStore.Server.DTO.Customer
{
    public class CustomerBaseRequest
    {
        public required string Name { get; set; }
        public required string Code { get; set; }
        public string? Address { get; set; }
    }
}
