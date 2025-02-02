namespace Basket.Data.Repository
{
	public class BasketRepository(BasketDbContext dbContext) : IBasketRepository
	{
		public async Task<ShoppingCart> CreateBasket(ShoppingCart basket, CancellationToken cancellationToken)
		{
			dbContext.ShoppingCarts.Add(basket);
			await dbContext.SaveChangesAsync(cancellationToken);
			return basket;
		}

		public async Task<bool> DeleteBasket(string username, CancellationToken cancellationToken = default)
		{
			var basket = await GetBasket(username, false, cancellationToken);
			dbContext.ShoppingCarts.Remove(basket);
			await dbContext.SaveChangesAsync(cancellationToken);

			return true;
		}

		public async Task<ShoppingCart> GetBasket(string username, bool asnoTracking = true, CancellationToken cancellationToken = default)
		{
			var query = dbContext.ShoppingCarts
						.Include(x => x.Items)
						.Where(x => x.UserName == username);

			if (asnoTracking)
			{
				query.AsNoTracking();
			}
			var basket = await query.SingleOrDefaultAsync(cancellationToken);

			return basket ?? throw new BasketNotFoundException(username);
		}

		public async Task<int> SaveChangesAsync(string? username = null, CancellationToken cancellationToken = default)
		{
			return await dbContext.SaveChangesAsync(cancellationToken);
		}
	}
}
