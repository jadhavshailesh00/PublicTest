﻿using Interview.Model;

namespace Interview.Service.Token
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
