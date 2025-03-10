using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Customer;
using OnlineStore.Server.Services.Customer;

namespace OnlineStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController(ICustomerService customerService, ILogger<CustomersController> logger) : ControllerBase
    {
        private readonly ICustomerService _customerService = customerService;
        private readonly ILogger<CustomersController> _logger = logger;

        [Authorize(Roles = "Manager")]
        [HttpPut(template: "update/{id}")]
        public async Task<ActionResult<bool>> Update(Guid id, [FromBody] CustomerRequest customer)
        {
            try
            {
                bool result = await _customerService.Update(id, customer);
                if (result) return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе Update.");
                return StatusCode(500);
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpGet(template: "getpage")]
        public async Task<ActionResult<ResponseList<CustomerResponse>>> GetPage([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                ResponseList<CustomerResponse> result = await _customerService.GetPage(pageNumber, pageSize);
                if (result is null) return BadRequest();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе GetPage.");
                return StatusCode(500);
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpGet(template: "getone")]
        public async Task<ActionResult<CustomerResponse>> GetOneByCriteria(CustomerFilterCriteria criteria)
        {
            try
            {
                CustomerResponse? result = await _customerService.GetOneByCriteria(criteria);
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
