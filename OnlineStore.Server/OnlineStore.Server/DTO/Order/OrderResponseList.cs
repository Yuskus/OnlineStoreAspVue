namespace OnlineStore.Server.DTO.Order
{
    public class OrderResponseList
    {
        public IEnumerable<OrderResponse> OrderResponses { get; set; }
        public int TotalCount { get; set; }

        public OrderResponseList()
        {
            OrderResponses = [];
            TotalCount = 0;
        }
        public OrderResponseList(IEnumerable<OrderResponse> orderResponses, int totalCount)
        {
            OrderResponses = orderResponses;
            TotalCount = totalCount;
        }
    }
}
