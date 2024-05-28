using System.Collections.Concurrent;

public class MyOAuthProvider : OAuthAuthorizationServerProvider
{
    private static ConcurrentDictionary<string, string> _tokenDictionary = new ConcurrentDictionary<string, string>();

    public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
    {
        using (UserRepo repo = new UserRepo())
        {
            var user = repo.ValidateUser(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "Username or Password is incorrect!");
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));

            foreach (var role in user.Roles.Split(','))
            {
                identity.AddClaim(new Claim(ClaimTypes.Role, role.Trim()));
            }

            var ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
            var token = context.Options.AccessTokenFormat.Protect(ticket);

            // Store the token in the dictionary
            _tokenDictionary[token] = token;

            context.Validated(ticket);
        }
    }

    public static bool ValidateToken(string token)
    {
        return _tokenDictionary.ContainsKey(token);
    }
}
