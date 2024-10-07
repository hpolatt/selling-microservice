using System;
using Web.ApiGateway.Extensions;
using Web.ApiGateway.Models.Catalog;
using Web.ApiGateway.Services.Interfaces;

namespace Web.ApiGateway.Services;

public class CatalogService : ICatalogService
{
    private readonly IHttpClientFactory httpClientFactory;

    public CatalogService(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }

    public async Task<CatalogItem> GetCatalogItemAsync(int id)
    {
        var client = httpClientFactory.CreateClient("catalog");
        var response = await client.GetResponseAsync<CatalogItem>($"items/{id}");
        return response;

    }

    public async Task<IEnumerable<CatalogItem>> GetCatalogItemAsync(IEnumerable<int> ids)
    {
        var client = httpClientFactory.CreateClient("catalog");
        var response = await client.GetResponseAsync<IEnumerable<CatalogItem>>($"items?ids={string.Join(",", ids)}");
        return response;
    }
}
