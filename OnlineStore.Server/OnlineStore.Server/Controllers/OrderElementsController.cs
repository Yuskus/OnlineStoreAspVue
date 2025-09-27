using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Server.DTO.OrderElement;
using OnlineStore.Server.Services.OrderElement;

namespace OnlineStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderElementsController(IOrderElementService orderElementService) : ControllerBase
    {
        private readonly IOrderElementService _orderElementService = orderElementService;

        [Authorize]
        [HttpPost(template: "add")]
        public async Task<ActionResult<Guid>> Create([FromBody] OrderElementRequest orderElement)
        {
            var result = await _orderElementService.Create(orderElement);

            return result is not null
                ? Ok(result.Value)
                : BadRequest();
        }

        [Authorize]
        [HttpPut(template: "update/{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateOrderElementRequest orderElement)
        {
            var result = await _orderElementService.Update(id, orderElement);

            return result
                ? Ok(result)
                : BadRequest();
        }

        [Authorize]
        [HttpDelete(template: "delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var result = await _orderElementService.Delete(id);

            return result
                ? Ok(result)
                : BadRequest();
        }

        [Authorize]
        [HttpGet(template: "getbyorderid/{id}")]
        public async Task<ActionResult<IEnumerable<OrderElementResponse>>> GetAllByOrderId(Guid id)
        {
            var result = await _orderElementService.GetAllByOrderId(id);

            return result is not null
                ? Ok(result)
                : BadRequest();
        }
    }
}
