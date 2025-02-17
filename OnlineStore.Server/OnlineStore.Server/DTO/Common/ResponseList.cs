namespace OnlineStore.Server.DTO.Common
{
    public class ResponseList<T>(IEnumerable<T> responses, int totalCount)
    {
        public IEnumerable<T> Responses { get; set; } = responses;
        public int TotalCount { get; set; } = totalCount;

        public ResponseList() : this([], 0) { }
    }
}
