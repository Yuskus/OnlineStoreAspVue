namespace OnlineStore.Server.DTO.Customer
{
    public class CustomerResponseList
    {
        public IEnumerable<CustomerResponse> CustomerResponses { get; set; }
        public int TotalCount { get; set; }

        public CustomerResponseList()
        {
            CustomerResponses = [];
            TotalCount = 0;
        }
        public CustomerResponseList(IEnumerable<CustomerResponse> responses, int totalCount)
        {
            CustomerResponses = responses;
            TotalCount = totalCount;
        }
    }
}
