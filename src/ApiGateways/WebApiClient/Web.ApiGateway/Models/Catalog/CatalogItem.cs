using System;

namespace Web.ApiGateway.Models.Catalog;

public class CatalogItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string PictureUri { get; set; }
}
