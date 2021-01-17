using System;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace EventBus.RabbitMq
{
    public class RabbitMqConnection : IRabbitMqConnection
    {
        public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;

        private readonly IConnectionFactory _connectionFactory;
        private IConnection _connection;
        private bool _disposed;

        public RabbitMqConnection(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
            if (!IsConnected)
            {
                TryConnect();
            }
        }

        public bool TryConnect()
        {
            try
            {
                _connection = _connectionFactory.CreateConnection();
            }
            catch (BrokerUnreachableException brokerUnreachableException)
            {
                Console.WriteLine($"Error by connection RabbitMqServer message: {brokerUnreachableException.Message}");
                Thread.Sleep(2000);
                _connection = _connectionFactory.CreateConnection();
            }

            if (IsConnected) _disposed = false;

            return IsConnected;
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No Rabbit Connected");
            }

            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (_disposed) return;

            try
            {
                _connection.Dispose();
            }
            catch (Exception e)
            {
                throw new ObjectDisposedException($"Connection can not be disposed message:{e.Message}");
            }
        }
    }
}