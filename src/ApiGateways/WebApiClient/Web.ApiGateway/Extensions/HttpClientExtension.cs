using System;
using System.Text;
using System.Text.Json;

namespace Web.ApiGateway.Extensions;

public static class HttpClientExtension
{
    public async static Task<TResult> PostGetResponseAsync<TResult, TValue>(this HttpClient client, string requestUri, TValue content)
    {

        var response = await client.PostAsJsonAsync(requestUri, content);
        return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<TResult>() : default;
    }

    public async static Task PostAsync<TValue>(this HttpClient client, string requestUri, TValue content)
    {
        await client.PostAsJsonAsync(requestUri, content);
    }

    public async static Task<T> GetResponseAsync<T>(this HttpClient client, string requestUri)
    {
        return await client.GetFromJsonAsync<T>(requestUri);
    }


}
