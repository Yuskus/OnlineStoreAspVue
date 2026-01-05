namespace OnlineStore.Server.Extensions.BCL.Exceptions
{
    public class EnvironmentVariableNotFoundException : Exception
    {
        public EnvironmentVariableNotFoundException() : base("Environment variable not found!") { }
        public EnvironmentVariableNotFoundException(string message) : base(message) { }
    }
}
