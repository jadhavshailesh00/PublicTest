using Interview.Entity.Token;
using Interview.Model;
using Interview.Service.Token;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Interview.App_Start
{
    public class CustomAuthorizationFilter : IAuthorizationFilter
    {
        private readonly OAuthConfig _oauthConfig;
        private  List<string> Roles=new List<string>();

        public CustomAuthorizationFilter(IOptions<OAuthConfig> oauthConfig)
        {
            _oauthConfig = oauthConfig.Value;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            Roles.Add("admin");
            Roles.Add("Developer");
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token == null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_oauthConfig.Key);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _oauthConfig.Issuer,
                    ValidateAudience = true,
                    ValidAudience = _oauthConfig.Audience,
                    ValidateLifetime = true
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var claimsIdentity = new ClaimsIdentity(jwtToken.Claims);

                var user = new ClaimsPrincipal(claimsIdentity);
                context.HttpContext.User = user;

                var hasRequiredScope = user.HasClaim(c => c.Type == "scope" && c.Value == "Product");
                if (!hasRequiredScope)
                {
                    context.Result = new ForbidResult();
                }

                var userRoles = jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

                if (Roles.Any(role => userRoles.Contains(role)))
                {
                    return; 
                }
                else
                {
                    context.Result = new ForbidResult();
                }

            }
            catch
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
