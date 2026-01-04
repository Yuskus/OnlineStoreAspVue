using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Server.DTO.Users;
using OnlineStore.Server.Services.Users;

namespace OnlineStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [AllowAnonymous]
        [HttpPost(template: "registercustomer")]
        public async Task<ActionResult<bool>> RegisterCustomer([FromBody] CustomerRegisterRequest customerRegisterRequest)
        {
            var result = await _userService.Register(customerRegisterRequest);

            return result
                ? Ok(result)
                : BadRequest();
        }

        [Authorize(Roles = "Manager")]
        [HttpPost(template: "registermanager")]
        public async Task<ActionResult<bool>> RegisterManager([FromBody] UserCredentialsRequest managerRegisterRequest)
        {
            var result = await _userService.Register(managerRegisterRequest);

            return result
                ? Ok(result)
                : BadRequest();
        }
    }
}
