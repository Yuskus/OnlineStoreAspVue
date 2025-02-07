using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        [HttpGet(template: "getpage")]
        public async Task<ActionResult<CustomerResponseList>> GetPageOfCustomers([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                CustomerResponseList result = await _customerService.GetPageOfCustomers(pageNumber, pageSize);
                if (result is null) return BadRequest();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе GetPageOfCustomers.");
                return StatusCode(500);
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpGet(template: "getbyid/{id}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerById(Guid id)
        {
            try
            {
                CustomerResponse? result = await _customerService.GetCustomerById(id);
                if (result is null) return BadRequest();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе GetCustomerById.");
                return StatusCode(500);
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpGet(template: "getbycode/{code}")]
        public async Task<ActionResult<CustomerResponse>> GetCustomerByCode(string code)
        {
            try
            {
                CustomerResponse? result = await _customerService.GetCustomerByCode(code);
                if (result is null) return BadRequest();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе GetCustomerByCode.");
                return StatusCode(500);
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpPut(template: "update/{id}")]
        public async Task<ActionResult<bool>> UpdateCustomer(Guid id, [FromBody] CustomerRequest customer)
        {
            try
            {
                bool result = await _customerService.UpdateCustomer(id, customer);
                if (result) return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе UpdateCustomer.");
                return StatusCode(500);
            }
        }
    }
}
