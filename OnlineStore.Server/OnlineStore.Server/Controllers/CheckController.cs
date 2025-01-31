using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OnlineStore.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet(template: "anon")]
        public ActionResult AmIAnon()
        {
            return Ok("Yes, you are Anon.");
        }

        [Authorize(Roles = "User")]
        [HttpGet(template: "user")]
        public ActionResult AmIUser()
        {
            return Ok("Yes, you are User.");
        }

        [Authorize(Roles = "Manager")]
        [HttpGet(template: "manager")]
        public ActionResult AmIManager()
        {
            return Ok("Yes, you are Manager.");
        }
    }
}
