using Interview.Entity;

namespace Interview.Service.Token
{
    public interface ITokenService
    {
        //string GenerateToken(User user, string clientId, string clientSecret, string scope);
        //bool ValidateToken(string token);
        //string GenerateAuthorizationCode(string clientId, string redirectUri);
        //bool ValidateAuthorizationCode(string code, string clientId, string redirectUri);
        string GenerateToken(User user);
    }
}
