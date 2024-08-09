using Interview.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Interview.Controllkers
{
    [ApiController]
    [Route("security")]
    public class SecurityController : ControllerBase
    {
        private readonly IJwtTokenService _jwtTokenService;

        public SecurityController(IJwtTokenService jwtTokenService)
        {
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost("createToken")]
        [AllowAnonymous]
        public IActionResult CreateToken([FromBody] User user)
        {
            if (user.UserName == "string" && user.Password == "string")
            {
                var token = _jwtTokenService.GenerateToken(user);
                return Ok(token);
            }

            return Unauthorized();
        }
    }
}

