namespace Interview.Entity.Token
{
    public class AuthorizationCode
    {
        public string Code { get; set; }
        public string ClientId { get; set; }
        public string RedirectUri { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
