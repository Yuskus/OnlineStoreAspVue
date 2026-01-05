namespace OnlineStore.Server.Constants.Orders
{
    public static class OrderStatuses
    {
        public const string New = "new";
        public const string Basket = "basket";
        public const string Completed = "completed";
        public const string InProgress = "in progress";

        public static bool Check(string? value) => value switch
        {
            New => true,
            Basket => true,
            Completed => true,
            InProgress => true,
            _ => false
        };
    }
}
