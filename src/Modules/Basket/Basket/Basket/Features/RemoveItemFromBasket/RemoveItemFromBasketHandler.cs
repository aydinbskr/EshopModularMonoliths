using Basket.Data.Repository;

namespace Basket.Basket.Features.RemoveItemFromBasket
{

	public record RemoveItemFromBasketCommand(string UserName, Guid ProductId)
	: ICommand<RemoveItemFromBasketResult>;
	public record RemoveItemFromBasketResult(Guid Id);
	public class RemoveItemFromBasketCommandValidator : AbstractValidator<RemoveItemFromBasketCommand>
	{

		public RemoveItemFromBasketCommandValidator()
		{
			RuleFor(x => x.UserName).NotEmpty().WithMessage("UserName is required");
			RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required");
		}
	}
	internal class RemoveItemFromBasketHandler(IBasketRepository repository)
		: ICommandHandler<RemoveItemFromBasketCommand, RemoveItemFromBasketResult>
	{
		public async Task<RemoveItemFromBasketResult> Handle(RemoveItemFromBasketCommand request, CancellationToken cancellationToken)
		{

			//var shoppingCart = await dbContext.ShoppingCarts
			//.Include(x => x.Items)
			//.SingleOrDefaultAsync(x => x.UserName == request.UserName, cancellationToken);

			//if (shoppingCart is null)
			//{
			//	throw new BasketNotFoundException(request.UserName);
			//}

			var shoppingCart = await repository.GetBasket(request.UserName, false, cancellationToken);

			shoppingCart.RemoveItem(request.ProductId);
			await repository.SaveChangesAsync(request.UserName,cancellationToken);
			return new RemoveItemFromBasketResult(shoppingCart.Id);
		}
	}
}
