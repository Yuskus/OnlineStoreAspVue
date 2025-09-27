using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Server.DTO.User;
using OnlineStore.Server.Services.User.RegistrationService;

namespace OnlineStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController(IRegistrationService registrationService) : ControllerBase
    {
        private readonly IRegistrationService _registrationService = registrationService;

        [AllowAnonymous]
        [HttpPost(template: "registercustomer")]
        public async Task<ActionResult<bool>> RegisterCustomer([FromBody] CustomerRegisterRequest customerRegisterRequest)
        {
            var result = await _registrationService.Register(customerRegisterRequest);

            return result
                ? Ok(result)
                : BadRequest();
        }

        [Authorize(Roles = "Manager")]
        [HttpPost(template: "registermanager")]
        public async Task<ActionResult<bool>> RegisterManager([FromBody] UserCredentialsRequest managerRegisterRequest)
        {
            var result = await _registrationService.Register(managerRegisterRequest);

            return result
                ? Ok(result)
                : BadRequest();
        }
    }
}
