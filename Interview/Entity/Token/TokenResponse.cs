namespace Interview.Entity.Token
{
    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; } = "Bearer";
        public int ExpiresIn { get; set; } = 3600;
        public string RefreshToken { get; set; }
    }
}
