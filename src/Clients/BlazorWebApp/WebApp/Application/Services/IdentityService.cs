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
    private readonly ILocalStorageService syncLocalStorageService;
    private readonly AuthenticationStateProvider authenticationStateProvider;

    public IdentityService(HttpClient httpClient, ILocalStorageService syncLocalStorageService, AuthenticationStateProvider authenticationStateProvider)
    {
        this.httpClient = httpClient;
        this.syncLocalStorageService = syncLocalStorageService;
        this.authenticationStateProvider = authenticationStateProvider;
    }
    public async Task<string> GetUserToken()
    {
       return await syncLocalStorageService.GetTokenAsync();
    }

    public async Task<string> GetUsername()
    {
       return await syncLocalStorageService.GetUsernameAsync();
    }

    public async Task<bool> IsLoggedIn() => !string.IsNullOrWhiteSpace(await GetUserToken());

    public async Task<bool> Login(string username, string password)
    {
        var req = new UserLoginRequest(username, password);

        var res = await httpClient.PostGetResponseAsync<UserLoginResponse, UserLoginRequest>("auth", req);
        if (res.Token is not null)
        {
            await syncLocalStorageService.SetTokenAsync(res.Token);
            await syncLocalStorageService.SetUsernameAsync(res.UserName);
            ((AuthStateProvider)authenticationStateProvider).NotifyUserLogin(res.UserName);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", res.Token);
            return true;
        }
        return false;
    }

    public async Task Logout()
    {
        await syncLocalStorageService.RemoveItemAsync("token");
        await syncLocalStorageService.RemoveItemAsync("username");
        ((AuthStateProvider)authenticationStateProvider).NotifyUserLogout();

        httpClient.DefaultRequestHeaders.Authorization = null;
    }
}
