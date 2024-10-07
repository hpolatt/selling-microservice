using System;

namespace WebApp.Application.Services.Interfaces;

public interface IIdentityService
{

    Task<string> GetUsername();

    Task<string> GetUserToken();

    Task<bool> IsLoggedIn();

    Task<bool> Login(string username, string token);

    Task Logout();

}
