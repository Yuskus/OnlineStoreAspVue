using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Orders;
using OnlineStore.Server.Services.Orders;

namespace OnlineStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IOrderService orderService) : ControllerBase
    {
        private readonly IOrderService _orderService = orderService;

        [Authorize]
        [HttpPost(template: "add")]
        public async Task<ActionResult<Guid>> Create([FromBody] OrderRequest order)
        {
            var result = await _orderService.Create(order);

            return result is not null
                ? Ok(result.Value)
                : BadRequest();
        }

        [Authorize(Roles = "Manager")]
        [HttpPut(template: "update/{id}")]
        public async Task<ActionResult<bool>> Update(Guid id, [FromBody] OrderRequest order)
        {
            var result = await _orderService.Update(id, order);

            return result
                ? Ok(result)
                : BadRequest();
        }

        [Authorize]
        [HttpDelete(template: "delete/{id}")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            var result = await _orderService.Delete(id);

            return result
                ? Ok(result)
                : BadRequest();
        }

        [Authorize(Roles = "User, Manager")]
        [HttpGet(template: "getbasket/{customerId}")]
        public async Task<ActionResult<OrderResponse?>> GetBasketOrder(Guid customerId)
        {
            var result = await _orderService.GetBasketOrder(customerId);
            
            return result is not null
                ? Ok(result)
                : BadRequest();
        }

        [Authorize(Roles = "User")]
        [HttpPatch(template: "placeanorder/{orderId}")]
        public async Task<ActionResult<bool>> PlaceAnOrder(Guid orderId)
        {
            var result = await _orderService.PlaceAnOrder(orderId);

            return result
                ? Ok(result)
                : BadRequest();
        }

        [Authorize]
        [HttpGet(template: "getpage")]
        public async Task<ActionResult<ResponseList<OrderResponse>>> GetPage([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var result = await _orderService.GetPage(new PageInfo
            {
                Number = pageNumber,
                Size = pageSize
            });
            
            return result is not null
                ? Ok(result)
                : BadRequest();
        }

        [Authorize]
        [HttpGet(template: "getpagebycriteria")]
        public async Task<ActionResult<ResponseList<OrderResponse>>> GetPageByCriteria(OrderFilterCriteria criteria, [FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var result = await _orderService.GetPageByCriteria(criteria, new PageInfo
            {
                Number = pageNumber,
                Size = pageSize
            });
            
            return result is not null
                ? Ok(result)
                : BadRequest();
        }

        [Authorize]
        [HttpGet(template: "getone")]
        public async Task<ActionResult<OrderResponse>> GetOneByCriteria(OrderFilterCriteria criteria)
        {
            var result = await _orderService.GetOneByCriteria(criteria);
            
            return result is not null
                ? Ok(result)
                : BadRequest();
        }
    }
}
