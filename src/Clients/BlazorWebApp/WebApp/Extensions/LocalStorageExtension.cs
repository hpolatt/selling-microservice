using System;
using Blazored.LocalStorage;

namespace WebApp.Extensions;

public static class LocalStorageExtension
{
    public static string GetUsername(this ISyncLocalStorageService localStorage)
    {
        return localStorage.GetItem<string>("username") ?? string.Empty;
    }

    public async static Task<string> GetUsernameAsync(this ILocalStorageService localStorage)
    {
        return await localStorage.GetItemAsync<string>("username") ?? string.Empty;
    }

    public static void SetUsername(this ISyncLocalStorageService localStorage, string username)
    {
        localStorage.SetItem("username", username);
    }

    public async static Task SetUsernameAsync(this ILocalStorageService localStorage, string username)
    {
        await localStorage.SetItemAsync("username", username);
    }

    public static string GetToken(this ISyncLocalStorageService localStorage)
    {
        try
        {
            return localStorage.GetItem<string>("token") ?? string.Empty;

        }
        catch (InvalidOperationException)
        {
            return string.Empty;
        }
    }

    public async static Task<string> GetTokenAsync(this ILocalStorageService localStorage)
    {
        try
        {
            return await localStorage.GetItemAsync<string>("token") ?? string.Empty;

        }
        catch (InvalidOperationException)
        {
            return string.Empty;
        }

    }

    public static void SetToken(this ISyncLocalStorageService localStorage, string token)
    {
        localStorage.SetItem("token", token);
    }

    public async static Task SetTokenAsync(this ILocalStorageService localStorage, string token)
    {
        await localStorage.SetItemAsync("token", token);
    }


}
