namespace OnlineStore.Server.Validation.Order
{
    public class OrderValidator
    {
        public static bool CheckGuid(Guid? guid)
        {
            return guid != null && guid != Guid.Empty;
        }

        public static bool CheckDates(string orderDate, string? shipmentDate)
        {
            if (DateOnly.TryParse(orderDate, out DateOnly orderDateOnly))
            {
                if (shipmentDate == null)
                {
                    return true;
                }
                else if (DateOnly.TryParse(shipmentDate, out DateOnly shipmentDateOnly))
                {
                    return orderDateOnly.ToDateTime(new TimeOnly()) <= shipmentDateOnly.ToDateTime(new TimeOnly());
                }
            }

            return false;
        }

        public static bool CheckPages(int pageNumber, int pageSize)
        {
            return pageNumber > 0 && pageSize > 0 && pageSize <= 50;
        }

        public static bool CheckStatus(string status)
        {
            return status == "completed" || status == "in progress" || status == "new";
        }
    }
}
