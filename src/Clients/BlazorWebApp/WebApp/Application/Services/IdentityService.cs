using System;
using System.Net.Http.Headers;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using WebApp.Application.Services.Interfaces;
using WebApp.Domain.Models.User;
using WebApp.Extensions;
using WebApp.Utils;

namespace WebApp.Application.Services;

public class IdentityService : IIdentityService
{
    private readonly HttpClient httpClient;
    private readonly ISyncLocalStorageService syncLocalStorageService;
    private readonly AuthenticationStateProvider authenticationStateProvider;

    public IdentityService(HttpClient httpClient, ISyncLocalStorageService syncLocalStorageService, AuthenticationStateProvider authenticationStateProvider)
    {
        this.httpClient = httpClient;
        this.syncLocalStorageService = syncLocalStorageService;
        this.authenticationStateProvider = authenticationStateProvider;
    }
    public string GetUserToken()
    {
       return syncLocalStorageService.GetToken();
    }

    public string GetUsername()
    {
       return syncLocalStorageService.GetUsername();
    }

    public bool IsLoggedIn() => !string.IsNullOrWhiteSpace(GetUserToken());

    public async Task<bool> Login(string username, string password)
    {
        var req = new UserLoginRequest(username, password);

        var res = await httpClient.PostGetResponseAsync<UserLoginResponse, UserLoginRequest>("api/identity/login", req);
        if (res.Token is not null) {
            syncLocalStorageService.SetToken(res.Token);
            syncLocalStorageService.SetUsername(res.UserName);
            ((AuthStateProvider)authenticationStateProvider).NotifyUserLogin(res.UserName);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", res.Token);
            return true;
        }
        return false;
    }

    public void Logout()
    {
        syncLocalStorageService.RemoveItem("token");
        syncLocalStorageService.RemoveItem("username");
        ((AuthStateProvider)authenticationStateProvider).NotifyUserLogout();

        httpClient.DefaultRequestHeaders.Authorization = null;
    }
}
