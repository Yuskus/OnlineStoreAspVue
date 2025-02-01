namespace OnlineStore.Server.DTO.User
{
    public class UserResponseList
    {
        public IEnumerable<UserResponse> UserResponses { get; set; }
        public int TotalCount { get; set; }

        public UserResponseList()
        {
            UserResponses = [];
            TotalCount = 0;
        }
        public UserResponseList(IEnumerable<UserResponse> userResponses, int totalCount)
        {
            UserResponses = userResponses;
            TotalCount = totalCount;
        }
    }
}
