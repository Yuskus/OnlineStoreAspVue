﻿using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Item;
using System.Collections.Immutable;

namespace OnlineStore.Server.Services.Item
{
    public interface IItemService
    {
        Task<ResponseList<ItemResponse>> GetPageOfItems(int pageNumber, int pageSize);
        Task<ResponseList<ItemResponse>> GetPageOfItemsByCategory(string category, int pageNumber, int pageSize);
        ImmutableSortedSet<string> GetAllCategories();
        Task<ItemResponse?> GetItemById(Guid id);
        Task<ItemResponse?> GetItemByCode(string code);
        Task<ItemResponse?> GetItemByName(string name);
        Task<Guid?> CreateItem(ItemRequest item);
        Task<bool> UpdateItem(Guid id, ItemRequest item);
        Task<bool> DeleteItem(Guid id);
    }
}
