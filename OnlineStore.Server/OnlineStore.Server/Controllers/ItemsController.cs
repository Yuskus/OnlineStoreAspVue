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

        [Authorize(Roles = "Manager")]
        [HttpPost(template: "add")]
        public async Task<ActionResult<Guid>> Create([FromBody] ItemRequest item)
        {
            var result = await _itemService.Create(item);

            return result is not null
                ? Ok(result.Value)
                : BadRequest();
        }

        [Authorize(Roles = "Manager")]
        [HttpPut(template: "update/{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] ItemRequest item)
        {
            var result = await _itemService.Update(id, item);

            return result
                ? Ok(result)
                : BadRequest();
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete(template: "delete/{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _itemService.Delete(id);
            
            return result
                ? Ok(result)
                : BadRequest();
        }

        [Authorize]
        [HttpGet(template: "getpage")]
        public async Task<ActionResult<ResponseList<ItemResponse>>> GetPage(
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize)
        {
            var result = await _itemService.GetPage(pageNumber, pageSize);
            
            return result is not null
                ? Ok(result)
                : BadRequest();
        }

        [Authorize]
        [HttpGet(template: "getpagebycriteria")]
        public async Task<ActionResult<ResponseList<ItemResponse>>> GetPageByCriteria(
            [FromBody] ItemFilterCriteria criteria,
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize)
        {
            var result = await _itemService.GetPageByCriteria(criteria, pageNumber, pageSize);
            
            return result is not null
                ? Ok(result)
                : BadRequest();
        }

        [Authorize]
        [HttpGet(template: "getcategories")]
        public ActionResult<ImmutableSortedSet<string>> GetAllCategories()
        {
            var result = _itemService.GetAllCategories();
            
            return result is not null
                ? Ok(result)
                : BadRequest();
        }

        [Authorize]
        [HttpGet(template: "getone")]
        public async Task<ActionResult<ItemResponse>> GetOneByCriteria([FromBody] ItemFilterCriteria criteria)
        {
            var result = await _itemService.GetOneByCriteria(criteria);
            
            return result is not null
                ? Ok(result)
                : BadRequest();
        }
    }
}
