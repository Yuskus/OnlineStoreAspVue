using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Order;
using OnlineStore.Server.Services.Order;

namespace OnlineStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IOrderService orderService, ILogger<OrdersController> logger) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;
        private readonly ILogger<OrdersController> _logger = logger;

        [Authorize]
        [HttpPost(template: "add")]
        public async Task<ActionResult<Guid>> Create([FromBody] OrderRequest order)
        {
            try
            {
                Guid? result = await _orderService.Create(order);
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
        public async Task<ActionResult<bool>> Update(Guid id, [FromBody] OrderRequest order)
        {
            try
            {
                bool result = await _orderService.Update(id, order);
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
        public async Task<ActionResult<bool>> Delete(Guid id) //?? roles?
        {
            try
            {
                bool result = await _orderService.Delete(id);
                if (result) return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе Delete.");
                return StatusCode(500);
            }
        }

        [Authorize(Roles = "User, Manager")]
        [HttpGet(template: "getbasket/{customerId}")]
        public async Task<ActionResult<OrderResponse?>> GetBasketOrder(Guid customerId)
        {
            try
            {
                OrderResponse? result = await _orderService.GetBasketOrder(customerId);
                if (result is null) return BadRequest();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе GetBasketOrder.");
                return StatusCode(500);
            }
        }

        [Authorize(Roles = "User")]
        [HttpPatch(template: "placeanorder/{orderId}")]
        public async Task<ActionResult<bool>> PlaceAnOrder(Guid orderId)
        {
            try
            {
                bool result = await _orderService.PlaceAnOrder(orderId);
                if (result) return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе PlaceAnOrder.");
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpGet(template: "getpage")]
        public async Task<ActionResult<ResponseList<OrderResponse>>> GetPage([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                ResponseList<OrderResponse> result = await _orderService.GetPage(pageNumber, pageSize);
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
        public async Task<ActionResult<ResponseList<OrderResponse>>> GetPageByCriteria(OrderFilterCriteria criteria, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                ResponseList<OrderResponse> result = await _orderService.GetPageByCriteria(criteria, pageNumber, pageSize);
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
        [HttpGet(template: "getone")]
        public async Task<ActionResult<OrderResponse>> GetOneByCriteria(OrderFilterCriteria criteria)
        {
            try
            {
                OrderResponse? result = await _orderService.GetOneByCriteria(criteria);
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
