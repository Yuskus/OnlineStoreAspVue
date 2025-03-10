using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Server.DTO.Common;
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
        [HttpPost(template: "registercustomer")]
        public async Task<ActionResult<bool>> RegisterCustomer([FromBody] CustomerRegisterRequest customerRegisterRequest)
        {
            try
            {
                _registrationService.CreateTransaction();

                bool result = await _registrationService.Register(customerRegisterRequest);

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
                _logger.LogError(ex, "Ошибка при запросе RegisterCustomer.");
                return StatusCode(500);
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpPost(template: "registermanager")]
        public async Task<ActionResult<bool>> RegisterManager([FromBody] UserCredentialsRequest managerRegisterRequest)
        {
            try
            {
                bool result = await _userService.RegisterManager(managerRegisterRequest);
                if (result) return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе RegisterManager.");
                return StatusCode(500);
            }
        }

        [Authorize(Roles = "Manager")]
        [HttpPut(template: "update/{username}")]
        public async Task<ActionResult> Update(string username, [FromBody] UserRequest userRequest)
        {
            try
            {
                bool result = await _userService.Update(username, userRequest);
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
        [HttpDelete(template: "delete/{username}")]
        public async Task<ActionResult> Delete(string username)
        {
            try
            {
                bool result = await _userService.Delete(username);
                if (result) return Ok(result);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе Delete.");
                return StatusCode(500);
            }
        }

        [AllowAnonymous]
        [HttpPost(template: "login")]
        public async Task<ActionResult<LoginResponse>> Authenticate([FromBody] UserCredentialsRequest loginRequest)
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

        [Authorize(Roles = "Manager")]
        [HttpGet(template: "getpage")]
        public async Task<ActionResult<ResponseList<UserResponse>>> GetPage([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            try
            {
                ResponseList<UserResponse> result = await _userService.GetPage(pageNumber, pageSize);
                if (result is null) return BadRequest();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при запросе GetPage.");
                return StatusCode(500);
            }
        }
    }
}
