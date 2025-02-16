
namespace Catalog.Products.Features.GetProductById
{
	//public record GetProductByIdQuery(Guid Id):IQuery<GetProductByIdResult>;

	//public record GetProductByIdResult(ProductDto Product);

	public class GetProductByIdHandler(CatalogDbContext dbContext) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
	{
		public async Task<GetProductByIdResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
		{
			var product = await dbContext.Products
								.AsNoTracking()
								.SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
			if(product is null)
			{
				throw new ProductNotFoundException(request.Id);
			}

			var productDto = product.Adapt<ProductDto>();

			return new GetProductByIdResult(productDto);
		}
	}
}
