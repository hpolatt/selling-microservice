using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using WebApp.Extensions;

namespace WebApp.Utils;

public class AuthStateProvider : AuthenticationStateProvider
{
    private readonly ILocalStorageService localStorage;
    private readonly HttpClient client;
    private readonly AuthenticationState anonymous;

    public AuthStateProvider(ILocalStorageService localStorage, HttpClient httpClient)
    {
        this.localStorage = localStorage;
        this.client = httpClient;
        this.anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string apiToken = await localStorage.GetTokenAsync();

        if (string.IsNullOrEmpty(apiToken)) return anonymous;

        string userName = await localStorage.GetUsernameAsync();

        ClaimsIdentity identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, userName)
        }, "jwtAuthType");

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public void NotifyUserLogin(string userName) {
        ClaimsIdentity identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, userName)
        }, "jwtAuthType");

        var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyUserLogout() {
        var authState = Task.FromResult(anonymous);
        NotifyAuthenticationStateChanged(authState);
    }
}
