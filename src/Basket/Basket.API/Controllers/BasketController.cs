using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using EventBus.RabbitMq.Common;
using EventBus.RabbitMq.Events;
using EventBus.RabbitMq.Producer;
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
        private readonly IMapper _mapper;
        private readonly EventBusRabbitMqProducer _eventBusRabbitMqProducer;

        public BasketController(IBasketRepository basketRepository, ILogger<BasketController> basketControllerLogger,
            IMapper mapper, EventBusRabbitMqProducer eventBusRabbitMqProducer)
        {
            _basketRepository = basketRepository;
            _basketControllerLogger = basketControllerLogger;
            _mapper = mapper;
            _eventBusRabbitMqProducer = eventBusRabbitMqProducer;
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
        public async Task<ActionResult> DeleteBasket(string userName)
        {
            var result = await _basketRepository.DeleteBasket(userName);
            return Ok(result);
        }

        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int) HttpStatusCode.Accepted)]
        [ProducesResponseType((int) HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CheckoutBasket([FromBody] BasketCheckout basketCheckout)
        {
            var basket = await _basketRepository.GetBasket(basketCheckout.UserName);
            if (basket == null) return BadRequest();

            var basketRemoved = await _basketRepository.DeleteBasket(basketCheckout.UserName);
            if (!basketRemoved) return BadRequest();

            var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckout);
            eventMessage.RequestId = Guid.NewGuid();
            eventMessage.TotalPrice = basket.TotalPrice;

            try
            {
                _eventBusRabbitMqProducer.PublishBasketCheckout(EventBusConstants.BasketCheckoutQueue, eventMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine($"error sending eventbusmessage message: {e.Message}");
                throw;
            }

            return Accepted();
        }
    }
}