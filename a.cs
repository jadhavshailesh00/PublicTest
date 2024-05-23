// SimpleAuthorizationServerProvider.cs
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
{
    private readonly Dictionary<string, string> _apiKeys;

    public SimpleAuthorizationServerProvider(Dictionary<string, string> apiKeys)
    {
        _apiKeys = apiKeys;
    }

    public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
    {
        context.Validated(); // Validate all clients for simplicity
    }

    public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
    {
        // Validate the user credentials here (e.g., against a database)
        if (context.UserName == "user" && context.Password == "password")
        {
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));

            // Generate API key
            var apiKey = GenerateApiKey();
            _apiKeys[context.UserName] = apiKey;

            identity.AddClaim(new Claim("api_key", apiKey));
            
            // Generate access token
            var ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
            context.Validated(ticket);
        }
        else
        {
            context.SetError("invalid_grant", "The user name or password is incorrect.");
        }
    }

    private string GenerateApiKey()
    {
        return Guid.NewGuid().ToString("N");
    }
}









// TokenController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.Owin.Security.OAuth;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class TokenController : ControllerBase
{
    private readonly OAuthAuthorizationServerOptions _options;

    public TokenController(OAuthAuthorizationServerOptions options)
    {
        _options = options;
    }

    [HttpPost]
    [ProducesResponseType(typeof(TokenResponse), 200)]
    public async Task<IActionResult> GenerateToken([FromBody] TokenRequest request)
    {
        // Create OAuth context
        var context = new OAuthGrantResourceOwnerCredentialsContext(HttpContext, _options, request.Username, request.Password, null);

        // Call GrantResourceOwnerCredentials method in SimpleAuthorizationServerProvider
        await _options.Provider.GrantResourceOwnerCredentials(context);

        // If authentication failed, return error
        if (context.HasError)
        {
            return BadRequest(context.Error);
        }

        // If authentication succeeded, return access token
        return Ok(new TokenResponse
        {
            AccessToken = context.TokenEndpointResponse.AccessToken,
            TokenType = "bearer",
            ExpiresIn = (int)context.TokenEndpointResponse.ExpiresIn.TotalSeconds
        });
    }
}








