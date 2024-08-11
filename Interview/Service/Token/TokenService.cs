using Interview.Entity;
using Interview.Model;
using Microsoft.Extensions.Configuration;
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
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, "user"),
                new Claim(ClaimTypes.Email, "Email")
            };

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
    }
}
