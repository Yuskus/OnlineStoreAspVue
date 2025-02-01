namespace OnlineStore.Server.DTO.Item
{
    public class ItemResponseList
    {
        public IEnumerable<ItemResponse> ItemResponses { get; set; }
        public int TotalCount { get; set; }

        public ItemResponseList()
        {
            ItemResponses = [];
            TotalCount = 0;
        }
        public ItemResponseList(IEnumerable<ItemResponse> itemResponses, int totalCount)
        {
            ItemResponses = itemResponses;
            TotalCount = totalCount;
        }
    }
}
