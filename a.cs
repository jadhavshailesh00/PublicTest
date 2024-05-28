
using System;
using System.Collections.Generic;

namespace TokenStorage
{
    // Define a class to represent user sessions
    public class UserSession
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public DateTime Expiry { get; set; }
    }

    // Define a class to manage token storage
    public class TokenManager
    {
        private Dictionary<string, UserSession> _sessions;

        public TokenManager()
        {
            _sessions = new Dictionary<string, UserSession>();
        }

        // Method to generate and store a token for a user
        public string GenerateToken(string userId)
        {
            // Generate a random token (you may want to use a library for better randomness)
            string token = Guid.NewGuid().ToString();

            // Set the token expiry time (for simplicity, let's set it to 1 hour from now)
            DateTime expiry = DateTime.UtcNow.AddHours(1);

            // Store the token in the session dictionary
            _sessions[token] = new UserSession { UserId = userId, Token = token, Expiry = expiry };

            return token;
        }

        // Method to validate a token and retrieve the associated user ID
        public string ValidateToken(string token)
        {
            if (_sessions.ContainsKey(token))
            {
                UserSession session = _sessions[token];

                // Check if the token has expired
                if (session.Expiry > DateTime.UtcNow)
                {
                    return session.UserId;
                }
                else
                {
                    // Token has expired, remove it from storage
                    _sessions.Remove(token);
                }
            }
            return null; // Token not found or expired
        }

        // Method to revoke a token (e.g., logout)
        public void RevokeToken(string token)
        {
            if (_sessions.ContainsKey(token))
            {
                _sessions.Remove(token);
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            TokenManager tokenManager = new TokenManager();

            // Example usage:
            string userId = "123";
            string token = tokenManager.GenerateToken(userId);
            Console.WriteLine("Generated Token: " + token);

            // Validate the token
            string validatedUserId = tokenManager.ValidateToken(token);
            if (validatedUserId != null)
            {
                Console.WriteLine("Valid Token for User ID: " + validatedUserId);
            }
            else
            {
                Console.WriteLine("Invalid Token");
            }

            // Revoke the token (logout)
            tokenManager.RevokeToken(token);
            Console.WriteLine("Token Revoked");

            // Attempt to validate the revoked token
            validatedUserId = tokenManager.ValidateToken(token);
            if (validatedUserId != null)
            {
                Console.WriteLine("Valid Token for User ID: " + validatedUserId);
            }
            else
            {
                Console.WriteLine("Invalid Token (Revoked)");
            }
        }
    }
}
