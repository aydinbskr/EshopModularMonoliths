using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Data.Repository
{
	public interface IBasketRepository
	{
		Task<ShoppingCart> GetBasket(string username, bool asnoTracking = true, CancellationToken cancellationToken = default);
		Task<ShoppingCart> CreateBasket(ShoppingCart basket, CancellationToken cancellationToken);
		Task<bool> DeleteBasket(string username, CancellationToken cancellationToken = default);
		Task<int> SaveChangesAsync(string? username = null,CancellationToken cancellationToken = default);
	}
}
