using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Server.DTO.OrderElement;
using OnlineStore.Server.Services.OrderElement;

namespace OnlineStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderElementsController(IOrderElementService orderElementService, ILogger<OrderElementsController> logger) : ControllerBase
    {
        private readonly IOrderElementService _orderElementService = orderElementService;
        private readonly ILogger<OrderElementsController> _logger = logger;

        [Authorize]
        [HttpPost(template: "add")]
        public async Task<ActionResult<Guid>> Create([FromBody] OrderElementRequest orderElement)
        {
            try
            {
                Guid? result = await _orderElementService.Create(orderElement);
                if (result is null) return BadRequest();
                return Ok((Guid)result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе Create.");
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpPut(template: "update/{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateOrderElementRequest orderElement)
        {
            try
            {
                bool result = await _orderElementService.Update(id, orderElement);
                if (result) return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе Update.");
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpDelete(template: "delete/{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                bool result = await _orderElementService.Delete(id);
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
        [HttpGet(template: "getbyorderid/{id}")]
        public async Task<ActionResult<IEnumerable<OrderElementResponse>>> GetAllByOrderId(Guid id)
        {
            try
            {
                IEnumerable<OrderElementResponse> result = await _orderElementService.GetAllByOrderId(id);
                if (result is null) return BadRequest();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе GetAllByOrderId.");
                return StatusCode(500);
            }
        }
    }
}
