using System;

namespace WebApp.Domain.Models.User;

public class UserLoginResponse
{
    public string UserName { get; private set; }
    public string Token { get; private set; }
    public UserLoginResponse(string userName, string token)
    {
        UserName = userName;
        Token = token;
        
    }

}
