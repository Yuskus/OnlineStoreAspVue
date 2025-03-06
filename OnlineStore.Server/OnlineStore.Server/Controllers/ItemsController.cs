using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Item;
using OnlineStore.Server.Services.Item;
using System.Collections.Immutable;

namespace OnlineStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController(IItemService itemService, ILogger<ItemsController> logger) : ControllerBase
    {
        private readonly IItemService _itemService = itemService;
        private readonly ILogger<ItemsController> _logger = logger;

        [Authorize]
        [HttpGet(template: "getpage")]
        public async Task<ActionResult<ResponseList<ItemResponse>>> GetPageOfItems([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                ResponseList<ItemResponse> result = await _itemService.GetPageOfItems(pageNumber, pageSize);
                if (result is null) return BadRequest();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе GetPageOfItems.");
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpGet(template: "getpagebycategory/{category}")]
        public async Task<ActionResult<ResponseList<ItemResponse>>> GetPageOfItemsByCategory(string category, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                ResponseList<ItemResponse> result = await _itemService.GetPageOfItemsByCriteria(new() { Category = category }, pageNumber, pageSize);
                if (result is null) return BadRequest();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе GetItemsByCategory.");
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpGet(template: "getcategories")]
        public ActionResult<ImmutableSortedSet<string>> GetAllCategories()
        {
            try
            {
                ImmutableSortedSet<string> result = _itemService.GetAllCategories();
                if (result is null) return BadRequest();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе GetAllCategories.");
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpGet(template: "getbyid/{id}")]
        public async Task<ActionResult<ItemResponse>> GetItemById(Guid id)
        {
            try
            {
                ItemResponse? result = await _itemService.GetOneByCriteria(new() { Id = id });
                if (result is null) return BadRequest();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе GetItemById.");
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpGet(template: "getbycode/{code}")]
        public async Task<ActionResult<ItemResponse>> GetItemByCode(string code)
        {
            try
            {
                ItemResponse? result = await _itemService.GetOneByCriteria(new() { Code = code });
                if (result is null) return BadRequest();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе GetItemByCode.");
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpGet(template: "getbyname/{name}")]
        public async Task<ActionResult<ItemResponse>> GetItemByName(string name)
        {
            try
            {
                ItemResponse? result = await _itemService.GetOneByCriteria(new() { Name = name });
                if (result is null) return BadRequest();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе GetItemByName.");
                return StatusCode(500);
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpPost(template: "add")]
        public async Task<ActionResult<Guid>> CreateItem([FromBody] ItemRequest item)
        {
            try
            {
                Guid? result = await _itemService.CreateItem(item);
                if (result is null) return BadRequest();
                return Ok((Guid)result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе CreateItem.");
                return StatusCode(500);
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpPut(template: "update/{id}")]
        public async Task<ActionResult> UpdateItem(Guid id, [FromBody] ItemRequest item)
        {
            try
            {
                bool result = await _itemService.UpdateItem(id, item);
                if (result) return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе UpdateItem.");
                return StatusCode(500);
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete(template: "delete/{id}")]
        public async Task<ActionResult> DeleteItem(Guid id)
        {
            try
            {
                bool result = await _itemService.DeleteItem(id);
                if (result) return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе DeleteItem.");
                return StatusCode(500);
            }
        }
    }
}
