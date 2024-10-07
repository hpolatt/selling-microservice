using System;
using WebApp.Application.Services.Interfaces;
using WebApp.Domain.Models;
using WebApp.Domain.Models.Catalog;
using WebApp.Extensions;

namespace WebApp.Application.Services;

public class CatalogService : ICatalogService
{
    private readonly HttpClient apiClient;

    public CatalogService(HttpClient apiClient)
    {
        this.apiClient = apiClient;
    }

    public Task<PaginatedItemsViewModel<CatalogItem>> GetCatalogItemsAsync(int pageSize, int pageIndex, string? brand, string? types)
    {
        var res = apiClient.GetResponseAsync<PaginatedItemsViewModel<CatalogItem>>("/catalog/items");

        return res;
    }
}
