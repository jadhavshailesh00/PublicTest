using Interview.Entity;
using Interview.Service.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Interview.Controllkers
{
    /// <summary>
    /// Handles security-related operations such as token generation.
    /// </summary>
    [ApiController]
    [Route("security")]
    public class SecurityController : ControllerBase
    {
        private readonly ITokenService _TokenService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityController"/> class.
        /// </summary>
        /// <param name="TokenService">The token service.</param>
        public SecurityController(ITokenService TokenService)
        {
            _TokenService = TokenService;
        }

        /// <summary>
        /// Generates a token for a given user.
        /// </summary>
        /// <param name="user">The user details.</param>
        /// <returns>A token if the credentials are valid.</returns>
        /// <response code="200">Returns the token.</response>
        /// <response code="401">If the credentials are invalid.</response>
        [HttpPost("createToken")]
        [AllowAnonymous]
        public IActionResult CreateToken([FromBody] User user)
        {
            if (user.UserName == "shailesh" && user.Password == "password")
            {
                var token = _TokenService.GenerateToken(user);
                return Ok(token);
            }

            return Unauthorized();
        }
    }
}

