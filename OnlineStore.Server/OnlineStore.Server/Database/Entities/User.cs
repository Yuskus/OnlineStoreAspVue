namespace OnlineStore.Server.Database.Entities
{
    public class User : BaseEntity
    {
        public Guid? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }
        public required string Username { get; set; }
        public required byte[] Password { get; set; }
        public required byte[] Salt { get; set; }
        public UserRole Role { get; set; } = UserRole.User;
    }

    public enum UserRole
    {
        User,
        Manager
    }
}
