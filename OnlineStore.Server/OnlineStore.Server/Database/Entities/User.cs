namespace OnlineStore.Server.Database.Entities
{
    public class User
    {
        public Guid? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
        public required string Username { get; set; }
        public required byte[] Password { get; set; }
        public required byte[] Salt { get; set; }
        public int Role { get; set; } = (int)UserRole.User;
    }

    public enum UserRole
    {
        User,
        Manager
    }
}
