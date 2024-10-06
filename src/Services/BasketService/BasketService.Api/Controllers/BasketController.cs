using BasketService.Api.Core.Application.Repository;
using BasketService.Api.Core.Application.Services;
using BasketService.Api.Core.Domain.Models;
using BasketService.Api.IntegrationEvents.Events;
using EventBus.Base.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BasketService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IIdentityService _identityService;
        private readonly IEventBus _eventBus;
        private readonly ILogger<BasketController> _logger;

        public BasketController(IBasketRepository basketRepository, IIdentityService identityService, IEventBus eventBus, ILogger<BasketController> logger)
        {
            _basketRepository = basketRepository;
            _identityService = identityService;
            _eventBus = eventBus;
            _logger = logger;
        }


        [HttpGet]
        public IActionResult Get()
        {
            return Ok("BasketService Up and Running");
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]
        [Route("update")]
        [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basket)
        {
            await _basketRepository.UpdateBasketAsync(basket);
            return Ok(basket);
        }

        [HttpPost]
        [Route("additem")]
        [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
        public async Task<ActionResult<CustomerBasket>> AddItemToBasket([FromBody] BasketItem basketItem)
        {
            var userId = _identityService.GetUserName();
            var basket = await _basketRepository.GetBasketAsync(userId);
            if (basket == null)
            {
                basket = new CustomerBasket(userId);
            }

            basket.Items.Add(basketItem);
            await _basketRepository.UpdateBasketAsync(basket);
            return Ok(basket);
        }



        [HttpPost]
        [Route("checkout")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Checkout([FromBody] BasketCheckout basketCheckout, [FromHeader(Name = "x-requestid")] string requestId)
        {
            var userId = basketCheckout.Buyer;
            var basket = await _basketRepository.GetBasketAsync(userId);
            if (basket == null)
            {
                return BadRequest();
            }

            var eventMessage = new OrderCreatedIntegrationEvent(userId, _identityService.GetUserName(),
                basketCheckout.City, basketCheckout.Street, basketCheckout.State, basketCheckout.Country, basketCheckout.ZipCode,
                basketCheckout.CardNumber, basketCheckout.CardHolderName, basketCheckout.CardExpiration, basketCheckout.CardSecurityNumber,
                basketCheckout.CardTypeId, _identityService.GetUserName(), basket);

            try
            {
                _eventBus.Publish(eventMessage);
                
            }
            catch (System.Exception)
            {
                _logger.LogError("ERROR Publishing integration event: {IntegrationEventId} from {AppName}", eventMessage.Id, "BasketService");
                throw;
            }

            return Accepted();
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteBasket(string id)
        {
            var basket = await GetBasketById(id);
            if (basket is null) return NotFound();
            await _basketRepository.DeleteBasketAsync(id);
            return Ok();
        }

    }
}
