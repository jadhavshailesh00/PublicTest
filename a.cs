using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;
using System.Collections.Generic;

[assembly: OwinStartup(typeof(YourNamespace.Startup))]

namespace YourNamespace
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var apiKeys = new Dictionary<string, string>(); // Replace with secure store in production
            var oAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true, // Do not use this in production
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new SimpleAuthorizationServerProvider(apiKeys)
            };

            app.UseOAuthAuthorizationServer(oAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            app.Use<ApiKeyMiddleware>(apiKeys);
        }
    }
}



using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
{
    private readonly Dictionary<string, string> apiKeys;

    public SimpleAuthorizationServerProvider(Dictionary<string, string> apiKeys)
    {
        this.apiKeys = apiKeys;
    }

    public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
    {
        context.Validated(); // Validate all clients for simplicity
    }

    public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
    {
        var identity = new ClaimsIdentity(context.Options.AuthenticationType);

        // Validate the user credentials here (e.g., against a database)
        if (context.UserName == "user" && context.Password == "password")
        {
            identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));

            // Generate API key
            var apiKey = GenerateApiKey();
            apiKeys[context.UserName] = apiKey;

            identity.AddClaim(new Claim("api_key", apiKey));
            context.Validated(identity);
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





using Microsoft.Owin;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ApiKeyMiddleware : OwinMiddleware
{
    private readonly Dictionary<string, string> apiKeys;

    public ApiKeyMiddleware(OwinMiddleware next, Dictionary<string, string> apiKeys)
        : base(next)
    {
        this.apiKeys = apiKeys;
    }

    public override async Task Invoke(IOwinContext context)
    {
        if (context.Request.Headers.ContainsKey("Authorization"))
        {
            var token = context.Request.Headers.Get("Authorization").Split(' ')[1];
            var apiKeyClaim = context.Authentication.User.FindFirst("api_key");

            if (apiKeyClaim != null && apiKeys.ContainsValue(apiKeyClaim.Value))
            {
                await Next.Invoke(context);
                return;
            }
        }

        context.Response.StatusCode = 401;
        context.Response.ReasonPhrase = "Unauthorized";
    }
}





using System.Web.Http;

[Authorize]
public class ProtectedController : ApiController
{
    public IHttpActionResult Get()
    {
        return Ok("This is a protected resource");
    }
}



