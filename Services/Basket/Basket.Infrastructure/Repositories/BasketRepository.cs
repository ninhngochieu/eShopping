using Basket.Core.Entities;
using Basket.Core.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.Infrastructure.Repositories;

/// <summary>
/// Todo: 3.4.1 Setup distribute cache using redis
/// </summary>
public class BasketRepository : IBasketRepository
{
    private readonly IDistributedCache _redisCache;

    public BasketRepository(IDistributedCache redisCache)
    {
        _redisCache = redisCache;
    }
    
    /// <summary>
    /// Todo: 3.4.2 Get cache
    /// </summary>
    /// <param name="userName"></param>
    /// <returns></returns>
    public async Task<ShoppingCart> GetBasket(string userName)
    {
        var basket = await _redisCache.GetStringAsync(userName);
        if (string.IsNullOrEmpty(basket))
        {
            return null;
        }

        return JsonConvert.DeserializeObject<ShoppingCart>(basket);
    }

    /// <summary>
    /// Todo: 3.4.3 Set cache
    /// </summary>
    /// <param name="shoppingCart"></param>
    /// <returns></returns>
    public async Task<ShoppingCart> UpdateBasket(ShoppingCart shoppingCart)
    {
        await _redisCache.SetStringAsync(shoppingCart.UserName, JsonConvert.SerializeObject(shoppingCart));
        return await GetBasket(shoppingCart.UserName);
    }

    /// <summary>
    /// Todo: 3.4.3 Delete cache
    /// </summary>
    /// <param name="userName"></param>
    public async Task DeleteBasket(string userName)
    {
        await _redisCache.RemoveAsync(userName);
    }
}