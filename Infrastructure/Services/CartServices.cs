using Core.Entities;
using Core.Repository;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Infrastructure.Services
{
    public class CartServices(IConnectionMultiplexer redis) : ICartService
    {
        private readonly IDatabase _database = redis.GetDatabase();

        public async Task<bool> DeleteCartAsync(string key)
        {
            return await _database.KeyDeleteAsync(key);
        }

        public async Task<ShoppingCart?> GetCartAsync(string key)
        {
            RedisValue data = await _database.StringGetAsync(key);

            return data.IsNullOrEmpty ? null : JsonConvert.DeserializeObject<ShoppingCart>(data.ToString());
        }

        public async Task<ShoppingCart?> SetCartAsync(ShoppingCart cart)
        {
            bool created = await _database.StringSetAsync(cart.Id, JsonConvert.SerializeObject(cart), TimeSpan.FromDays(30));

            if (!created) return null;

            return await GetCartAsync(cart.Id);
        }
    }
}