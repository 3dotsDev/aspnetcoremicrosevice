using System.Threading.Tasks;
using Basket.API.Data.Interfaces;
using Basket.API.Entities;
using Basket.API.Repositories.Interfaces;
using Newtonsoft.Json;

namespace Basket.API.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IBasketContext _ctx;

        public BasketRepository(IBasketContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<BasketCart> GetBasket(string userName)
        {
            var basket = await _ctx.Redis.StringGetAsync(userName);

            return basket.IsNullOrEmpty ? null : JsonConvert.DeserializeObject<BasketCart>(basket);
        }

        public async Task<BasketCart> UpdateBasket(BasketCart basketCart)
        {
            var updated = await _ctx.Redis.StringSetAsync(basketCart.UserName, JsonConvert.SerializeObject(basketCart));

            if (!updated)
            {
                return null;
            }

            return await GetBasket(basketCart.UserName);
        }

        public async Task<bool> DeleteBasket(string userName)
        {
            return await _ctx.Redis.KeyDeleteAsync(userName);
        }
    }
}