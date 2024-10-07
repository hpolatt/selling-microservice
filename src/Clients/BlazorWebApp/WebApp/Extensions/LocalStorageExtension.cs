using System;
using Blazored.LocalStorage;

namespace WebApp.Extensions;

public static class LocalStorageExtension
{

    public async static Task<string> GetUsernameAsync(this ILocalStorageService localStorage)
    {
        try
        {
            var username = await localStorage.GetItemAsync<string>("username");
            if (string.IsNullOrEmpty(username)) return string.Empty;
            return username;
        }
        catch (InvalidOperationException ex)
        {
            return string.Empty;
        }

    }

    public async static Task SetUsernameAsync(this ILocalStorageService localStorage, string username)
    {
        await localStorage.SetItemAsync("username", username);
    }

    public async static Task<string> GetTokenAsync(this ILocalStorageService localStorage)
    {
        try
        {
            var token = await localStorage.GetItemAsync<string>("token");
            if (string.IsNullOrEmpty(token)) return string.Empty;
            return token;
        }
        catch (InvalidOperationException ex)
        {
            return string.Empty;
        }
    }

    public async static Task SetTokenAsync(this ILocalStorageService localStorage, string token)
    {
        await localStorage.SetItemAsync("token", token);
    }


}
