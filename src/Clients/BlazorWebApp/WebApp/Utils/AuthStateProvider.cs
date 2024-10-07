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
        String apiToken = await localStorage.GetTokenAsync();

        if (String.IsNullOrEmpty(apiToken))
            return anonymous;

        String userName = await localStorage.GetUsernameAsync();

        var cp = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
                new Claim(ClaimTypes.Name, userName)

            }, "jwtAuthType"));

        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiToken);

        return new AuthenticationState(cp);
    }

    public void NotifyUserLogin(string userName)
    {
        ClaimsIdentity identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, userName)
        }, "jwtAuthType");

        var authState = Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        NotifyAuthenticationStateChanged(authState);
    }

    public void NotifyUserLogout()
    {
        var authState = Task.FromResult(anonymous);
        NotifyAuthenticationStateChanged(authState);
    }
}
