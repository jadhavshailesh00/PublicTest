using Interview.Entity;
using Interview.Service.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Interview.Controllkers
{
    [ApiController]
    [Route("security")]
    public class SecurityController : ControllerBase
    {
        private readonly ITokenService _TokenService;

        public SecurityController(ITokenService TokenService)
        {
            _TokenService = TokenService;
        }

        [HttpPost("createToken")]
        [AllowAnonymous]
        public IActionResult CreateToken([FromBody] User user)
        {
            if (user.UserName == "string" && user.Password == "string")
            {
                var token = _TokenService.GenerateToken(user);
                return Ok(token);
            }

            return Unauthorized();
        }
    }
}

