using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Server.DTO.User;
using OnlineStore.Server.Services.User;
using OnlineStore.Server.Services.User.RegistrationService;

namespace OnlineStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(IUserService userService, ICustomerRegistrationService registrationService, ILogger<UsersController> logger) : ControllerBase
    {
        private readonly IUserService _userService = userService;
        private readonly ICustomerRegistrationService _registrationService = registrationService;
        private readonly ILogger<UsersController> _logger = logger;

        [AllowAnonymous]
        [HttpPost(template: "login")]
        public async Task<ActionResult<LoginResponse>> Authenticate([FromBody] LoginRequest loginRequest)
        {
            try
            {
                LoginResponse? result = await _userService.Authenticate(loginRequest);
                if (result is null) return BadRequest();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе Authenticate.");
                return StatusCode(500);
            }
        }

        [AllowAnonymous]
        [HttpPost(template: "register")]
        public async Task<ActionResult<bool>> RegisterUser([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                _registrationService.CreateTransaction();

                bool result = await _registrationService.RegisterUser(registerRequest);

                if (result)
                {
                    await _registrationService.Save();
                    _registrationService.Commit();
                    return Ok(result);
                }

                _registrationService.Rollback();
                return BadRequest();
            }
            catch (Exception ex)
            {
                _registrationService.Rollback();
                _logger.LogError(ex, "Ошибка при запросе RegisterUser.");
                return StatusCode(500);
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpPost(template: "addmanager")]
        public async Task<ActionResult<bool>> AddManager([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                bool result = await _userService.AddManager(registerRequest);
                if (result) return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе AddManager.");
                return StatusCode(500);
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpGet(template: "getall")]
        public async Task<ActionResult<UserResponseList>> GetPageOfUsersInfo([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                UserResponseList result = await _userService.GetPageOfUsersInfo(pageNumber, pageSize);
                if (result is null) return BadRequest();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе GetUsersInfo.");
                return StatusCode(500);
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpPut(template: "update")]
        public async Task<ActionResult> UpdateUser([FromBody] UserRequest userRequest)
        {
            try
            {
                bool result = await _userService.UpdateUser(userRequest);
                if (result) return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе UpdateUser.");
                return StatusCode(500);
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpDelete(template: "delete/{name}")]
        public async Task<ActionResult> DeleteUser(string name)
        {
            try
            {
                bool result = await _userService.DeleteUser(name);
                if (result) return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе DeleteUser.");
                return StatusCode(500);
            }
        }
    }
}
