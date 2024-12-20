﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Products.Features.GetProductById
{
	public record GetProductByIdQuery(Guid Id):IQuery<GetProductByIdResult>;

	public record GetProductByIdResult(ProductDto Product);

	public class GetProductByIdHandler(CatalogDbContext dbContext) : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
	{
		public async Task<GetProductByIdResult> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
		{
			var product = await dbContext.Products
								.AsNoTracking()
								.SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
			if(product is null)
			{
				throw new Exception($"Product not found:{request.Id}");
			}

			var productDto = product.Adapt<ProductDto>();

			return new GetProductByIdResult(productDto);
		}
	}
}
