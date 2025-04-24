namespace OnlineStore.Server.Services.User.RegistrationService
{
    public interface IRegistrationService<in T>
    {
        Task<bool> Register(T request);
    }
}
