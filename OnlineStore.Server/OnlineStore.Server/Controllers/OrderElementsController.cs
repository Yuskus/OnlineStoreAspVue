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
        [HttpGet(template: "getbyid/{id}")]
        public async Task<ActionResult<IEnumerable<OrderElementResponse>>> GetOrderElementsByOrderId(Guid id)
        {
            try
            {
                IEnumerable<OrderElementResponse> result = await _orderElementService.GetOrderElementsByOrderId(id);
                if (result is null) return BadRequest();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе GetOrderElementById.");
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpPost(template: "add")]
        public async Task<ActionResult<Guid>> CreateOrderElement([FromBody] OrderElementRequest orderElement)
        {
            try
            {
                Guid? result = await _orderElementService.CreateOrderElement(orderElement);
                if (result is null) return BadRequest();
                return Ok((Guid)result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе CreateOrderElement.");
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpPut(template: "update/{id}")]
        public async Task<ActionResult> UpdateOrderElement(Guid id, [FromBody] OrderElementRequest orderElement)
        {
            try
            {
                bool result = await _orderElementService.UpdateOrderElement(id, orderElement);
                if (result) return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе UpdateOrderElement.");
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpDelete(template: "delete/{id}")]
        public async Task<ActionResult> DeleteOrderElement(Guid id)
        {
            try
            {
                bool result = await _orderElementService.DeleteOrderElement(id);
                if (result) return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе DeleteOrderElement.");
                return StatusCode(500);
            }
        }
    }
}
