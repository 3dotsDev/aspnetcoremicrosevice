using System;
using System.Text;
using EventBus.RabbitMq.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace EventBus.RabbitMq.Producer
{
    public class EventBusRabbitMqProducer
    {
        private readonly IRabbitMqConnection _connection;

        public EventBusRabbitMqProducer(IRabbitMqConnection connection)
        {
            _connection = connection;
        }

        public void PublishBasketCheckout(string queueName, BasketCheckoutEvent @event)
        {
            using (var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false,
                    arguments: null);
                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                IBasicProperties properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.DeliveryMode = 2;
                
                channel.ConfirmSelect();
                channel.BasicPublish(exchange: "", routingKey: queueName, mandatory: true, basicProperties: properties, body: body);
                channel.WaitForConfirmsOrDie();

                channel.BasicAcks += (sender, eventArgs) =>
                {
                    Console.WriteLine("Sent RabbitMQ");
                    //implement ack handle
                };
                channel.ConfirmSelect();
            }
        }
    }
}