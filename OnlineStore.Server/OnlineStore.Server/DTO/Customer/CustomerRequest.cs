namespace OnlineStore.Server.DTO.Customer
{
    public class CustomerRequest
    {
        public required string Name { get; set; }
        public required string Code { get; set; }
        public string? Address { get; set; }
        public int Discount { get; set; } = 0;
    }
}
