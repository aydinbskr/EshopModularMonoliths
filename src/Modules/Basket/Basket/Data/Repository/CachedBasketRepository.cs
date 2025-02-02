using Basket.Data.JsonConverters;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Basket.Data.Repository
{
	public class CachedBasketRepository(IBasketRepository repository, IDistributedCache cache) : IBasketRepository
	{
		private readonly JsonSerializerOptions _options = new JsonSerializerOptions
		{
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
			DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
			Converters = { new ShoppingCartConverter(), new ShoppingCartItemConverter() }
		};
		public async Task<ShoppingCart> CreateBasket(ShoppingCart basket, CancellationToken cancellationToken)
		{
			await repository.CreateBasket(basket, cancellationToken);
			await cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);

			return basket;
		}

		public async Task<bool> DeleteBasket(string username, CancellationToken cancellationToken = default)
		{
			await repository.DeleteBasket(username, cancellationToken);
			await cache.RemoveAsync(username, cancellationToken);

			return true;
		}

		public async Task<ShoppingCart> GetBasket(string username, bool asnoTracking = true, CancellationToken cancellationToken = default)
		{
			if(!asnoTracking)
			{
				return await repository.GetBasket(username, false, cancellationToken);
			}

			var cachedBasket = await cache.GetStringAsync(username, cancellationToken);
			if (!string.IsNullOrEmpty(cachedBasket))
			{
				return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket,_options)!;
			}

			var basket = await repository.GetBasket(username, asnoTracking, cancellationToken);

			await cache.SetStringAsync(username, JsonSerializer.Serialize(basket, _options), cancellationToken);
			return basket;
		}

		public async Task<int> SaveChangesAsync(string? username = null, CancellationToken cancellationToken = default)
		{
			var result = await repository.SaveChangesAsync(username,cancellationToken);

			if(username is not null)
			{
				await cache.RemoveAsync(username, cancellationToken);
			}

			return result;
		}
	}
}
