using System;
using System.Net.Http.Headers;
using Blazored.LocalStorage;
using WebApp.Extensions;

namespace WebApp.Infrastructure;

public class AuthTokenHandler : DelegatingHandler
{
    private readonly ILocalStorageService localStorage;

    public AuthTokenHandler(ILocalStorageService localStorage)
    {
        this.localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (localStorage is not null) {
            string apiToken = await localStorage.GetTokenAsync();

            if (!string.IsNullOrEmpty(apiToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiToken);
            }
        }
        return await base.SendAsync(request, cancellationToken);
    }

}
