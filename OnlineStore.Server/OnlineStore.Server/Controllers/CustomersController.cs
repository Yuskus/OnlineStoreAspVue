using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Customers;
using OnlineStore.Server.Services.Customers;

namespace OnlineStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController(ICustomerService customerService) : ControllerBase
    {
        private readonly ICustomerService _customerService = customerService;

        // uses
        [Authorize(Roles = "Manager")]
        [HttpPut("update/{id:guid}")]
        public async Task<ActionResult<bool>> Update(Guid id, [FromBody] CustomerRequest customer)
        {
            var result = await _customerService.Update(id, customer);

            return result
                ? Ok(result)
                : BadRequest();
        }

        [Authorize(Roles = "Manager")]
        [HttpGet("getpage")]
        public async Task<ActionResult<ResponseList<CustomerResponse>>> GetPage(
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize)
        {
            var result = await _customerService.GetPage(new PageInfo
            {
                Number = pageNumber,
                Size = pageSize
            });

            return result is not null
                ? Ok(result)
                : BadRequest();
        }
    }
}
