using System.Text;
using AutoMapper;
using EventBus.RabbitMq;
using EventBus.RabbitMq.Common;
using EventBus.RabbitMq.Events;
using MediatR;
using Newtonsoft.Json;
using Ordering.Application.Commands;
using Ordering.Core.Repositories;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;

namespace Ordering.API.RabbitMq
{
    public class EventBusRabbitMqConsumer
    {
        private readonly IRabbitMqConnection _connection;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        // private readonly IOrderRepository _orderRepository;

        public EventBusRabbitMqConsumer(IRabbitMqConnection connection, IMediator mediator, IMapper mapper,
            IOrderRepository orderRepository)
        {
            _connection = connection;
            _mediator = mediator;
            _mapper = mapper;
            // _orderRepository = orderRepository;
        }

        public void Consume()
        {
            var channel = _connection.CreateModel();
            channel.QueueDeclare(queue: EventBusConstants.BasketCheckoutQueue, durable: false, exclusive: false,
                autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += ReceivedEvent;

            channel.BasicConsume(EventBusConstants.BasketCheckoutQueue, true, consumer);
        }

        private async void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            if (e.RoutingKey == EventBusConstants.BasketCheckoutQueue)
            {
                var message = Encoding.UTF8.GetString(e.Body.Span);
                var basketCheckoutEvent = JsonConvert.DeserializeObject<BasketCheckoutEvent>(message);

                var command = _mapper.Map<CheckOutOrderCommand>(basketCheckoutEvent);
                var result = await _mediator.Send(command);
            }
        }

        public void Disconnect()
        {
            _connection.Dispose();
        }
    }
}