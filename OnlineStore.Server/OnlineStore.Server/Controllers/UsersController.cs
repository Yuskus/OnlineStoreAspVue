using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Server.DTO.Common;
using OnlineStore.Server.DTO.User;
using OnlineStore.Server.Services.User;

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
            LoginResponse? result = await _userService.Authenticate(loginRequest);

            return result is not null ? Ok(result) : BadRequest();
        }

        [Authorize(Roles = "Manager")]
        [HttpPut(template: "update/{username}")]
        public async Task<ActionResult> Update(string username, [FromBody] UserRequest userRequest)
        {
            bool result = await _userService.Update(username, userRequest);

            return result ? Ok(result) : BadRequest();
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete(template: "delete/{username}")]
        public async Task<ActionResult> Delete(string username)
        {
            bool result = await _userService.Delete(username);

            return result ? Ok(result) : BadRequest();
        }

        [Authorize(Roles = "Manager")]
        [HttpGet(template: "getpage")]
        public async Task<ActionResult<ResponseList<UserResponse>>> GetPage([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            ResponseList<UserResponse> result = await _userService.GetPage(pageNumber, pageSize);

            return result is not null ? Ok(result) : BadRequest();
        }
    }
}
