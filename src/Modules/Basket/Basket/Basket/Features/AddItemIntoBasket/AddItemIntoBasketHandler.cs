using Basket.Data.Repository;
using Catalog.Contracts.Products.Features.GetProductById;

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
	internal class AddItemIntoBasketHandler(IBasketRepository repository, ISender sender)
		: ICommandHandler<AddItemIntoBasketCommand, AddItemIntoBasketResult>
	{
		public async Task<AddItemIntoBasketResult> Handle(AddItemIntoBasketCommand request, CancellationToken cancellationToken)
		{

			//var shoppingCart = await dbContext.ShoppingCarts
			//.Include(x => x.Items)
			//.SingleOrDefaultAsync(x => x.UserName == request.UserName, cancellationToken);

			//if (shoppingCart is null)
			//{
			//	throw new BasketNotFoundException(request.UserName);
			//}
			var shoppingCart = await repository.GetBasket(request.UserName, false, cancellationToken);

			var result = await sender.Send(new GetProductByIdQuery(request.ShoppingCartItem.ProductId));

			shoppingCart.AddItem(
			request.ShoppingCartItem.ProductId,
			request.ShoppingCartItem.Quantity,
			request.ShoppingCartItem.Color,
			result.Product.Price,
			result.Product.Name);
			//request.ShoppingCartItem.Price,
			//request.ShoppingCartItem.ProductName);

			await repository.SaveChangesAsync(request.UserName, cancellationToken);
			return new AddItemIntoBasketResult(shoppingCart.Id);
		}
	}

}
