using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.Users;
using OnlineStore.Server.Services.Users;

namespace OnlineStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        // uses
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Authenticate([FromBody] UserCredentialsRequest loginRequest)
        {
            var result = await _userService.Authenticate(loginRequest);

            return result is not null
                ? Ok(result)
                : BadRequest();
        }

        // uses
        [AllowAnonymous]
        [HttpPost("registercustomer")]
        public async Task<ActionResult<bool>> RegisterCustomer([FromBody] CustomerRegisterRequest customerRegisterRequest)
        {
            var result = await _userService.Register(customerRegisterRequest);

            return result
                ? Ok(result)
                : BadRequest();
        }

        // uses
        [Authorize(Roles = "Manager")]
        [HttpPost("registermanager")]
        public async Task<ActionResult<bool>> RegisterManager([FromBody] UserCredentialsRequest managerRegisterRequest)
        {
            var result = await _userService.Register(managerRegisterRequest);

            return result
                ? Ok(result)
                : BadRequest();
        }

        // uses
        [Authorize(Roles = "Manager")]
        [HttpPut("update/{username}")]
        public async Task<ActionResult> Update(string username, [FromBody] UserRequest userRequest)
        {
            var result = await _userService.Update(username, userRequest);

            return result
                ? Ok(result)
                : BadRequest();
        }

        // uses
        [Authorize(Roles = "Manager")]
        [HttpDelete("delete/{username}")]
        public async Task<ActionResult> Delete(string username)
        {
            var result = await _userService.Delete(username);

            return result
                ? Ok(result)
                : BadRequest();
        }

        // uses
        [Authorize(Roles = "Manager")]
        [HttpGet("getpage")]
        public async Task<ActionResult<ResponseList<UserResponse>>> GetPage(
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize)
        {
            var result = await _userService.GetPage(new PageInfo
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
