using Interview.Entity;
using Interview.Model;
using Interview.Service.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Interview.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class OAuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly OAuthConfig _oauthConfig;


        public OAuthController(ITokenService tokenService, IOptions<OAuthConfig> oauthConfig)
        {
            _tokenService = tokenService;
            _oauthConfig = oauthConfig.Value;
        }

        [HttpPost("Token")]
        public IActionResult Token(TokenRequest request)
        {
            if (string.IsNullOrEmpty(request.client_id) ||
                string.IsNullOrEmpty(request.GrantType) ||
                string.IsNullOrEmpty(request.client_secret))
            {
                return BadRequest(new
                {
                    ClientId = string.IsNullOrEmpty(request.client_id) ? "The ClientId field is required." : null,
                    GrantType = string.IsNullOrEmpty(request.GrantType) ? "The GrantType field is required." : null,
                    ClientSecret = string.IsNullOrEmpty(request.client_secret) ? "The ClientSecret field is required." : null
                });
            }

            if (request.client_id != _oauthConfig.ClientId ||
                request.client_secret != _oauthConfig.ClientSecret)
            {
                return Unauthorized("Invalid client credentials.");
            }

            // Check GrantType
            if (request.GrantType != "authorization_code" && request.GrantType != "password")
            {
                return BadRequest("Unsupported grant type.");
            }

            // Generate the token
            var user = new User { UserName = request.Username };
            var token = _tokenService.GenerateToken(user);

            return Ok(new { access_token = token, token_type = "bearer" });
        }
    }
}