namespace OnlineStore.Server.Authorization.Abstractions
{
    public interface ITokenGenerator
    {
        string GenerateToken(string username, string role);
    }
}
