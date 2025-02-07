namespace OnlineStore.Server.DTO.Customer
{
    public class CustomerRequest : CustomerBaseRequest
    {
        public int Discount { get; set; } = 0;
    }
}
