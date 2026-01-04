using Moq;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Items;
using OnlineStore.Server.Repositories.Items;

namespace OnlineStore.Server.Tests.Services.Item
{
    public class ItemServiceFixture
    {
        public Guid Guid_Unexists { get; private set; } = Guid.NewGuid();
        public Guid ItemGuid_Exists { get; private set; } = Guid.NewGuid();
        public string ItemName_Exists { get; private set; } = "Name exists";
        public string ItemCode_Exists { get; private set; } = "11-1111-YU11";
        public double ItemPrice_Exists { get; private set; } = 100.0;
        public string ItemCategory_Exists { get; private set; } = "Category exists";
        public int ResponseTotal { get; private set; }
        public IEnumerable<ItemResponse> ResponseList { get; private set; }

        public ItemServiceFixture()
        {
            ResponseList =
            [
                new()
                {
                    Id = ItemGuid_Exists,
                    Code = ItemCode_Exists,
                    Name = ItemName_Exists,
                    Category = ItemCategory_Exists
                }
            ];
            ResponseTotal = ResponseList.Count();
        }

        public Mock<IItemRepository> CreateMockRepository()
        {
            var mockRepository = new Mock<IItemRepository>();

            //create

            mockRepository
                .Setup(x => x.Create(
                    It.Is<ItemRequest>(x => x.Code == ItemCode_Exists)))
                .ReturnsAsync(ItemGuid_Exists);

            //update

            mockRepository
                .Setup(x => x.Update(
                    ItemGuid_Exists,
                    It.Is<ItemRequest>(x => x.Name == ItemName_Exists
                        || x.Code == ItemCode_Exists
                        || x.Price == ItemPrice_Exists
                        || x.Category == ItemCategory_Exists))) //??
                .ReturnsAsync(true);

            mockRepository
                .Setup(x => x.Update(
                    Guid_Unexists,
                    It.IsAny<ItemRequest>()))
                .ReturnsAsync(false);

            //delete

            mockRepository
                .SetupSequence(x => x.Delete(ItemGuid_Exists))
                .ReturnsAsync(true)
                .ReturnsAsync(false);

            mockRepository
                .Setup(x => x.Delete(Guid_Unexists))
                .ReturnsAsync(false);

            //get

            mockRepository
                .Setup(x => x.GetOneByCriteria(
                    It.Is<ItemFilterCriteria>(x => x.Id == ItemGuid_Exists
                        || x.Name == ItemName_Exists
                        || x.Code == ItemCode_Exists
                        || x.Category == ItemCategory_Exists
                        || (x.Id == null && x.Name == null && x.Code == null && x.Category == null))))
                .ReturnsAsync(() => ResponseList.FirstOrDefault());

            mockRepository
                .Setup(x => x.GetOneByCriteria(
                    It.Is<ItemFilterCriteria>(x => x.Id != ItemGuid_Exists
                        && x.Name != ItemName_Exists
                        && x.Code != ItemCode_Exists
                        && x.Category != ItemCategory_Exists
                        && !(x.Id == null && x.Name == null && x.Code == null && x.Category == null))))
                .ReturnsAsync(() => null);

            mockRepository
                .Setup(x => x.GetPage(
                    It.Is<PageInfo>(p => p.Number == 1 && p.Size == 12)))
                .ReturnsAsync(() => new(ResponseList, ResponseTotal));

            mockRepository
                .Setup(x => x.GetPage(
                    It.Is<PageInfo>(p => p.Number == 1 && p.Size == 20)))
                .ReturnsAsync(() => new(ResponseList, ResponseTotal));

            mockRepository
                .Setup(x => x.GetPage(
                    It.Is<PageInfo>(p => p.Number == 2 && p.Size == 3)))
                .ReturnsAsync(() => new([], ResponseTotal));

            mockRepository
                .Setup(x => x.GetPageByCriteria(
                    It.Is<ItemFilterCriteria>(x => x.Id == ItemGuid_Exists
                        || x.Name == ItemName_Exists
                        || x.Code == ItemCode_Exists
                        || x.Category == ItemCategory_Exists
                        || (x.Id == null && x.Name == null && x.Code == null && x.Category == null)),
                    It.Is<PageInfo>(p => p.Number == 1 && p.Size == 12)))
                .ReturnsAsync(() => new(ResponseList, ResponseTotal));

            mockRepository
                .Setup(x => x.GetPageByCriteria(
                    It.Is<ItemFilterCriteria>(x => x.Id == ItemGuid_Exists
                        || x.Name == ItemName_Exists
                        || x.Code == ItemCode_Exists
                        || x.Category == ItemCategory_Exists
                        || (x.Id == null && x.Name == null && x.Code == null && x.Category == null)),
                    It.Is<PageInfo>(p => p.Number == 1 && p.Size == 20)))
                .ReturnsAsync(() => new(ResponseList, ResponseTotal));

            mockRepository
                .Setup(x => x.GetPageByCriteria(
                    It.Is<ItemFilterCriteria>(x => x.Id == ItemGuid_Exists
                        || x.Name == ItemName_Exists
                        || x.Code == ItemCode_Exists
                        || x.Category == ItemCategory_Exists
                        || (x.Id == null && x.Name == null && x.Code == null && x.Category == null)),
                    It.Is<PageInfo>(p => p.Number == 2 && p.Size == 3)))
                .ReturnsAsync(() => new([], ResponseTotal));

            mockRepository
                .Setup(x => x.GetPageByCriteria(
                    It.Is<ItemFilterCriteria>(x => x.Id != ItemGuid_Exists
                        && x.Name != ItemName_Exists
                        && x.Code != ItemCode_Exists
                        && x.Category != ItemCategory_Exists
                        && !(x.Id == null && x.Name == null && x.Code == null && x.Category == null)),
                    It.IsAny<PageInfo>()))
                .ReturnsAsync(() => new([], 0));

            return mockRepository;
        }
    }

    [CollectionDefinition("ItemServiceCollection")]
    public class ItemServiceCollection : ICollectionFixture<ItemServiceFixture> { }
}
