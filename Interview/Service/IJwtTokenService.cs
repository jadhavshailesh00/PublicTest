namespace Interview.Service
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
