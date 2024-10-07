using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.ApiGateway.Models.Basket;
using Web.ApiGateway.Services.Interfaces;

namespace Web.ApiGateway.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class BasketController : ControllerBase
    {
        private readonly ICatalogService catalogService;
        private readonly IBasketService basketService;

        public BasketController(ICatalogService catalogService, IBasketService basketService)
        {
            this.catalogService = catalogService;
            this.basketService = basketService;
        }
        
        [HttpPost]
        [Route("items")]
        public async Task<IActionResult> AddItemToBasketAsync([FromBody] AddBasketItemRequest request)
        {  
            if (request is null || request.Quantity <= 0)
            {
                return BadRequest("Invalid Payload");
            }
            var item = await catalogService.GetCatalogItemAsync(request.CatalogItemId);

            var currentBasket = await basketService.GetBasketAsync(request.BasketId);


            var existingItem = currentBasket.Items.FirstOrDefault(i => i.ProductId == request.CatalogItemId);
            if (existingItem is not null)
            {
                existingItem.Quantity += request.Quantity;
            }
            else
            {
                currentBasket.Items.Add(new BasketDataItem
                {
                    ProductId = item.Id,
                    ProductName = item.Name,
                    UnitPrice = item.Price,
                    Quantity = request.Quantity,
                    PictureUrl = item.PictureUri,
                    Id = Guid.NewGuid().ToString()
                });
            }

            await basketService.UpdateBasketAsync(currentBasket);

            
            return Ok();
        }
    }
}
