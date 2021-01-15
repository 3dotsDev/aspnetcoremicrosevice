using Basket.API.Data.Interfaces;
using StackExchange.Redis;

namespace Basket.API.Data
{
    public class BasketContext : IBasketContext
    {
        private readonly ConnectionMultiplexer _connectionMultiplexer;
        
        public IDatabase Redis { get; }

        public BasketContext(ConnectionMultiplexer connectionMultiplexer)
        {
            _connectionMultiplexer = connectionMultiplexer;
            Redis = connectionMultiplexer.GetDatabase();
        }
    }
}