using OnlineStore.Server.DTO.Customer;

namespace OnlineStore.Server.DTO.User
{
    public class CustomerRegisterRequest
    {
        public Guid? Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required CustomerBaseRequest CustomerInfo { get; set; }
    }
}
