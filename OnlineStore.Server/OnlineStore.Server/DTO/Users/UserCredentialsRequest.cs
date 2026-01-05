namespace OnlineStore.Server.DTO.Users
{
    public class UserCredentialsRequest
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
