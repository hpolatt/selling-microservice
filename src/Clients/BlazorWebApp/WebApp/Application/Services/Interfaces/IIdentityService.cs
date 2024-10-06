using System;

namespace WebApp.Application.Services.Interfaces;

public interface IIdentityService
{

    string GetUsername();

    string GetUserToken();

    bool IsLoggedIn();

    Task<bool> Login(string username, string token);

    void Logout();

}
