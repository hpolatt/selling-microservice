using System.Net;
using CatalogService.Api.Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Api.Controllers;

[Route("api/")]
[ApiController]
public class PictureController : ControllerBase
{
    private readonly IWebHostEnvironment env;
    private readonly CatalogContext catalogContext;

    public PictureController(IWebHostEnvironment env, CatalogContext catalogContext)
    {
        this.env = env;
        this.catalogContext = catalogContext;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok("App and Running");
    }

    [HttpGet]
    [Route("catalog/picture/{catalogItemId:int}")]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    // GET: /<controller>/
    public async Task<ActionResult> GetImageAsync(int catalogItemId)
    {
        if (catalogItemId <= 0)
        {
            return BadRequest();
        }

        var item = await catalogContext.CatalogItems.SingleOrDefaultAsync(ci => ci.Id == catalogItemId);

        if (item != null)
        {
            var webRoot = env.WebRootPath;
            var path = Path.Combine(webRoot, item.PictureFileName);

            string imageFileExtension = Path.GetExtension(item.PictureFileName);
            string mimetype = GetImageMimeTypeFromImageFileExtension(imageFileExtension);

            var buffer = await System.IO.File.ReadAllBytesAsync(path);

            return File(buffer, mimetype);
        }

        return NotFound();
    }

    private string GetImageMimeTypeFromImageFileExtension(string extension)
    {

        return extension switch
        {
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".jpg" => "image/jpeg",
            ".jpeg" => "image/jpeg",
            ".bmp" => "image/bmp",
            ".tiff" => "image/tiff",
            ".wmf" => "image/wmf",
            ".jp2" => "image/jp2",
            ".svg" => "image/svg+xml",
            _ => "application/octet-stream"
        };
    }
}

// 