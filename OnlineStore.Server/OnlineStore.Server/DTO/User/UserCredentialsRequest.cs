namespace OnlineStore.Server.DTO.User
{
    public class UserCredentialsRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
