namespace OnlineStore.Server.DTO.Customer
{
    public class CustomerResponseList
    {
        public IEnumerable<CustomerResponse> Responses { get; set; }
        public int TotalCount { get; set; }

        public CustomerResponseList()
        {
            Responses = [];
            TotalCount = 0;
        }
        public CustomerResponseList(IEnumerable<CustomerResponse> responses, int totalCount)
        {
            Responses = responses;
            TotalCount = totalCount;
        }
    }
}
