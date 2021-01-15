using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BasketController : ControllerBase
    {
        private readonly IBasketRepository _basketRepository;
        private readonly ILogger<BasketController> _basketControllerLogger;

        public BasketController(IBasketRepository basketRepository, ILogger<BasketController> basketControllerLogger)
        {
            _basketRepository = basketRepository;
            _basketControllerLogger = basketControllerLogger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BasketCart), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<ActionResult<BasketCart>> GetBasket(string userName)
        {
            var basket = await _basketRepository.GetBasket(userName);
            if (basket == null)
            {
                _basketControllerLogger.LogError($"Basket for username : {userName}, not found");
                return NotFound();
            }

            return Ok(basket);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BasketCart), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> UpdateBasket([FromBody] BasketCart basketCart)
        {
            var basket = await _basketRepository.UpdateBasket(basketCart);
            return Ok(basket);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(bool), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<BasketCart>> DeleteBasket(string userName)
        {
            var result = await _basketRepository.DeleteBasket(userName);
            return Ok(result);
        }
    }
}