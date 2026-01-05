using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Server.DTO.OrderElements;
using OnlineStore.Server.Services.OrderElements;

namespace OnlineStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderElementsController(IOrderElementService orderElementService) : ControllerBase
    {
        private readonly IOrderElementService _orderElementService = orderElementService;

        // uses
        [Authorize]
        [HttpPost("add")]
        public async Task<ActionResult<Guid>> Create([FromBody] OrderElementRequest orderElement)
        {
            var result = await _orderElementService.Create(orderElement);

            return result is not null
                ? Ok(result.Value)
                : BadRequest();
        }

        // uses
        [Authorize]
        [HttpPut("update/{id:guid}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateOrderElementRequest orderElement)
        {
            var result = await _orderElementService.Update(id, orderElement);

            return result
                ? Ok(result)
                : BadRequest();
        }

        // uses
        [Authorize]
        [HttpDelete("delete/{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _orderElementService.Delete(id);

            return result
                ? Ok(result)
                : BadRequest();
        }

        // uses
        [Authorize]
        [HttpGet("getbyorderid/{id:guid}")]
        public async Task<ActionResult<IEnumerable<OrderElementResponse>>> GetAllByOrderId(Guid id)
        {
            var result = await _orderElementService.GetAllByOrderId(id);

            return result is not null
                ? Ok(result)
                : BadRequest();
        }
    }
}
