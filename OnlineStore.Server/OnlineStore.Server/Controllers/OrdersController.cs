using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Item;
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
        [HttpGet(template: "getpage")]
        public async Task<ActionResult<ResponseList<OrderResponse>>> GetPageOfOrders([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                ResponseList<OrderResponse> result = await _orderService.GetPage(pageNumber, pageSize);
                if (result is null) return BadRequest();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе GetPageOfOrders.");
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpGet(template: "getpagebycustomer/{id}")]
        public async Task<ActionResult<ResponseList<OrderResponse>>> GetPageOfOrdersByCustomerId(Guid id, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                ResponseList<OrderResponse> result = await _orderService.GetPageByCriteria(new() {  Id = id }, pageNumber, pageSize);
                if (result is null) return BadRequest();
                return Ok(result);
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе GetPageOfOrdersByCustomerId.");
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpGet(template: "getpagebystatus/{status}")]
        public async Task<ActionResult<ResponseList<OrderResponse>>> GetPageOfOrdersByStatus(string status, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                ResponseList<OrderResponse> result = await _orderService.GetPageByCriteria(new() { OrderStatus = status }, pageNumber, pageSize);
                if (result is null) return BadRequest();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе GetOrderByStatus.");
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpGet(template: "getbynumber/{number}")]
        public async Task<ActionResult<OrderResponse>> GetOrderByNumber(int number)
        {
            try
            {
                OrderResponse? result = await _orderService.GetOneByCriteria(new() { OrderNumber = number });
                if (result is null) return BadRequest();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе GetOrderByNumber.");
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

        [Authorize]
        [HttpPost(template: "add")]
        public async Task<ActionResult<Guid>> CreateOrder([FromBody] OrderRequest order)
        {
            try
            {
                Guid? result = await _orderService.Create(order);
                if (result is null) return BadRequest();
                return Ok((Guid)result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе CreateOrder.");
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

        [Authorize(Roles = "Manager")]
        [HttpPut(template: "update/{id}")]
        public async Task<ActionResult<bool>> UpdateOrder(Guid id, [FromBody] OrderRequest order)
        {
            try
            {
                bool result = await _orderService.Update(id, order);
                if (result) return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе UpdateOrder.");
                return StatusCode(500);
            }
        }

        [Authorize]
        [HttpDelete(template: "delete/{id}")]
        public async Task<ActionResult<bool>> DeleteOrder(Guid id)
        {
            try
            {
                bool result = await _orderService.Delete(id);
                if (result) return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе DeleteOrder.");
                return StatusCode(500);
            }
        }
    }
}
