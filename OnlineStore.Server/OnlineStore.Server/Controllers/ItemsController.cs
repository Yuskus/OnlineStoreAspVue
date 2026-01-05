using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Items;
using OnlineStore.Server.Services.Items;
using System.Collections.Immutable;

namespace OnlineStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController(IItemService itemService) : ControllerBase
    {
        private readonly IItemService _itemService = itemService;

        // uses
        [Authorize(Roles = "Manager")]
        [HttpPost("add")]
        public async Task<ActionResult<Guid>> Create([FromBody] ItemRequest item)
        {
            var result = await _itemService.Create(item);

            return result is not null
                ? Ok(result.Value)
                : BadRequest();
        }

        // uses
        [Authorize(Roles = "Manager")]
        [HttpPut("update/{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] ItemRequest item)
        {
            var result = await _itemService.Update(id, item);

            return result
                ? Ok(result)
                : BadRequest();
        }

        // uses
        [Authorize(Roles = "Manager")]
        [HttpDelete("delete/{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _itemService.Delete(id);
            
            return result
                ? Ok(result)
                : BadRequest();
        }

        // uses
        [Authorize]
        [HttpGet("getpage")]
        public async Task<ActionResult<ResponseList<ItemResponse>>> GetPage(
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize)
        {
            var result = await _itemService.GetPage(new PageInfo
            {
                Number = pageNumber,
                Size = pageSize
            });
            
            return result is not null
                ? Ok(result)
                : BadRequest();
        }

        // uses
        [Authorize]
        [HttpPost("getpagebycriteria")]
        public async Task<ActionResult<ResponseList<ItemResponse>>> GetPageByCriteria(
            [FromBody] ItemFilterCriteria criteria,
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize)
        {
            var result = await _itemService.GetPageByCriteria(criteria, new PageInfo
            {
                Number = pageNumber,
                Size = pageSize
            });
            
            return result is not null
                ? Ok(result)
                : BadRequest();
        }

        // uses
        [Authorize]
        [HttpGet("getcategories")]
        public ActionResult<ImmutableSortedSet<string>> GetAllCategories()
        {
            var result = _itemService.GetAllCategories();
            
            return result is not null
                ? Ok(result)
                : BadRequest();
        }
    }
}
