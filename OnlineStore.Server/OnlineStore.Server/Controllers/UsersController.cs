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

        [AllowAnonymous]
        [HttpPost(template: "login")]
        public async Task<ActionResult<LoginResponse>> Authenticate([FromBody] UserCredentialsRequest loginRequest)
        {
            var result = await _userService.Authenticate(loginRequest);

            return result is not null
                ? Ok(result)
                : BadRequest();
        }

        [Authorize(Roles = "Manager")]
        [HttpPut(template: "update/{username}")]
        public async Task<ActionResult> Update(string username, [FromBody] UserRequest userRequest)
        {
            var result = await _userService.Update(username, userRequest);

            return result
                ? Ok(result)
                : BadRequest();
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete(template: "delete/{username}")]
        public async Task<ActionResult> Delete(string username)
        {
            var result = await _userService.Delete(username);

            return result
                ? Ok(result)
                : BadRequest();
        }

        [Authorize(Roles = "Manager")]
        [HttpGet(template: "getpage")]
        public async Task<ActionResult<ResponseList<UserResponse>>> GetPage(
            [FromQuery] int pageNumber,
            [FromQuery] int pageSize)
        {
            var result = await _userService.GetPage(pageNumber, pageSize);

            return result is not null
                ? Ok(result)
                : BadRequest();
        }
    }
}
