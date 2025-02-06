using OnlineStore.Server.DTO.Customer;

namespace OnlineStore.Server.DTO.User
{
    public class CustomerRegisterRequest
    {
        public Guid? CustomerId { get; set; }
        public required CustomerRequest CustomerRequest { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
