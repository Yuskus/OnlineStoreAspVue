using OnlineStore.Server.DTO.Customer;

namespace OnlineStore.Server.DTO.User
{
    public class CustomerRegisterRequest : UserCredentialsRequest
    {
        public Guid? Id { get; set; }
        public required CustomerBaseRequest CustomerInfo { get; set; }
    }
}
