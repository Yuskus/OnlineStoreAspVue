using OnlineStore.Server.DTO.Orders;
using OnlineStore.Server.Validation.Customers;

namespace OnlineStore.Server.Validation.Orders
{
    public static class OrderValidator
    {
        public static bool CheckCriteria(OrderFilterCriteria criteria)
        {
            return criteria.Id != Guid.Empty
                && criteria.CustomerId != Guid.Empty
                && CheckOrderNumber(criteria.OrderNumber)
                && CheckStatus(criteria.OrderStatus);
        }

        public static bool CheckRequest(OrderRequest order)
        {
            return CustomerValidator.CheckGuid(order.CustomerId)
                && CheckDates(order.OrderDate, order.ShipmentDate)
                && CheckStatus(order.OrderStatus);
        }

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

        public static bool CheckOrderNumber(int? number)
        {
            return number is null || number >= 0;
        }

        public static bool CheckPages(int pageNumber, int pageSize)
        {
            return pageNumber > 0 && pageSize > 0 && pageSize <= 50;
        }

        public static bool CheckStatus(string? status)
        {
            return status is null
                || status == "completed" 
                || status == "in progress" 
                || status == "new" 
                || status == "basket";
        }
    }
}
