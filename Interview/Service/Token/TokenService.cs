using Interview.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Interview.Service.Token
{
    public class TokenService : ITokenService
    {
        private readonly OAuthConfig _oauthConfig;

        public TokenService(IOptions<OAuthConfig> oauthConfig)
        {
            _oauthConfig = oauthConfig.Value;
        }

        public string GenerateToken(User user)
        {
            var issuer = _oauthConfig.Issuer;
            var audience = _oauthConfig.Audience;
            var Scopes = _oauthConfig.Scopes;
            var key = Encoding.ASCII.GetBytes(_oauthConfig.Key);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("scope", Scopes),
                new Claim(ClaimTypes.Email, "Jadhavshailesh00@gmail.com")
            };

            var roles = FatchUserRoles(user);


            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(60),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }


        public List<string> FatchUserRoles(User user)
        {
            var claims = new List<string>();
            if (user != null)
            {
                if (user.UserName == "shailesh")
                {
                    claims.Add("admin");
                }
                else if (user.UserName == "ram")
                {
                    claims.Add("developer");
                }
            }
            return claims;
        }
    }
}
