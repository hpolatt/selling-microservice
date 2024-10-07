using System;
using WebApp.Domain.Models;
using WebApp.Domain.Models.Catalog;

namespace WebApp.Application.Services.Interfaces;

public interface ICatalogService
{
    Task<PaginatedItemsViewModel<CatalogItem>> GetCatalogItemsAsync(int pageSize, int pageIndex, string? brand, string? types);

}
