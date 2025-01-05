namespace Basket.Basket.Features.AddItemIntoBasket
{

	public record AddItemIntoBasketCommand(string UserName, ShoppingCartItemDto ShoppingCartItem)
			: ICommand<AddItemIntoBasketResult>;
	public record AddItemIntoBasketResult(Guid Id);
	public class AddItemIntoBasketCommandValidator : AbstractValidator<AddItemIntoBasketCommand>
	{
		public AddItemIntoBasketCommandValidator()
		{
			RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
			RuleFor(x => x.ShoppingCartItem.ProductId).NotEmpty().WithMessage("ProductId is required");
			RuleFor(x => x.ShoppingCartItem.Quantity).GreaterThan(0).WithMessage("Quantity must be greater zero");
		}
	}
	internal class AddItemIntoBasketHandler(BasketDbContext dbContext)
		: ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
	{
		public async Task<AddItemIntoBasketResult> Handle(AddItemIntoBasketCommand request, CancellationToken cancellationToken)
		{

			var shoppingCart = await dbContext.ShoppingCarts
			.Include(x => x.Items)
			.SingleOrDefaultAsync(x => x.UserName == request.UserName, cancellationToken);

			if (shoppingCart is null)
			{
				throw new BasketNotFoundException(request.UserName);
			}

			shoppingCart.AddItem(
			request.ShoppingCartItem.ProductId,
			request.ShoppingCartItem.Quantity,
			request.ShoppingCartItem.Color,
			request.ShoppingCartItem.Price,
			request.ShoppingCartItem.ProductName);

			await dbContext.SaveChangesAsync(cancellationToken);
			return new AddItemIntoBasketResult(shoppingCart.Id);
		}
	}

}
