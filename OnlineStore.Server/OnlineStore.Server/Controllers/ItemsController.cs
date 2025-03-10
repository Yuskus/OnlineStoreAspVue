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

        [Authorize(Roles = "Manager")]
        [HttpPost(template: "add")]
        public async Task<ActionResult<Guid>> Create([FromBody] ItemRequest item)
        {
            try
            {
                Guid? result = await _itemService.Create(item);
                if (result is null) return BadRequest();
                return Ok((Guid)result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе Create.");
                return StatusCode(500);
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpPut(template: "update/{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] ItemRequest item)
        {
            try
            {
                bool result = await _itemService.Update(id, item);
                if (result) return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе Update.");
                return StatusCode(500);
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete(template: "delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                bool result = await _itemService.Delete(id);
                if (result) return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе Delete.");
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpGet(template: "getpage")]
        public async Task<ActionResult<ResponseList<ItemResponse>>> GetPage([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                ResponseList<ItemResponse> result = await _itemService.GetPage(pageNumber, pageSize);
                if (result is null) return BadRequest();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе GetPage.");
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpGet(template: "getpagebycriteria")]
        public async Task<ActionResult<ResponseList<ItemResponse>>> GetPageByCriteria(ItemFilterCriteria criteria, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                ResponseList<ItemResponse> result = await _itemService.GetPageByCriteria(criteria, pageNumber, pageSize);
                if (result is null) return BadRequest();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе GetPageByCriteria.");
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
        [HttpGet(template: "getone")]
        public async Task<ActionResult<ItemResponse>> GetOneByCriteria(ItemFilterCriteria criteria)
        {
            try
            {
                ItemResponse? result = await _itemService.GetOneByCriteria(criteria);
                if (result is null) return BadRequest(); 
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе GetOneByCriteria.");
                return StatusCode(500);
            }
        }
    }
}
