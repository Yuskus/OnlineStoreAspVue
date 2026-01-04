namespace OnlineStore.Server.Constants.Orders
{
    public class OrderStatuses
    {
        public const string New = "new";
        public const string Basket = "basket";
        public const string NotExists = "not exists";
        public const string Completed = "completed";
        public const string InProgress = "in progress";

        public static bool Check(string? value) => value switch
        {
            New => true,
            Basket => true,
            NotExists => true,
            Completed => true,
            InProgress => true,
            _ => false
        };
    }
}
