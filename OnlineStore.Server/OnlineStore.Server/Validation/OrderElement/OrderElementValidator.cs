namespace OnlineStore.Server.Validation.OrderElement
{
    public class OrderElementValidator
    {
        public static bool CheckGuid(Guid? guid)
        {
            return guid != null && guid != Guid.Empty;
        }

        public static bool CheckCount(int count)
        {
            return count > 0;
        }

        public static bool CheckPrice(double price)
        {
            return price > 0;
        }
    }
}
