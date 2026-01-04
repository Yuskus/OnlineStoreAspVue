namespace OnlineStore.Server.DTO.Common
{
    public class PageInfo
    {
        public int Number { get; set; }
        public int Size { get; set; }

        public PageInfo() { }
        public PageInfo(int page, int size)
        {
            Number = page;
            Size = size;
        }
    }
}
