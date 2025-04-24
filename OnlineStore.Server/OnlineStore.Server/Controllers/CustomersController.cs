using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Customer;
using OnlineStore.Server.Services.Customer;

namespace OnlineStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController(ICustomerService customerService) : ControllerBase
    {
        private readonly ICustomerService _customerService = customerService;

        [Authorize(Roles = "Manager")]
        [HttpPut(template: "update/{id}")]
        public async Task<ActionResult<bool>> Update(Guid id, [FromBody] CustomerRequest customer)
        {
            bool result = await _customerService.Update(id, customer);

            return result ? Ok(result) : BadRequest();
        }

        [Authorize(Roles = "Manager")]
        [HttpGet(template: "getpage")]
        public async Task<ActionResult<ResponseList<CustomerResponse>>> GetPage([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            ResponseList<CustomerResponse> result = await _customerService.GetPage(pageNumber, pageSize);

            return result is not null ? Ok(result) : BadRequest();
        }

        [Authorize(Roles = "Manager")]
        [HttpGet(template: "getone")]
        public async Task<ActionResult<CustomerResponse>> GetOneByCriteria(CustomerFilterCriteria criteria)
        {
            CustomerResponse? result = await _customerService.GetOneByCriteria(criteria);
            
            return result is not null ? Ok(result) : BadRequest();
        }
    }
}
