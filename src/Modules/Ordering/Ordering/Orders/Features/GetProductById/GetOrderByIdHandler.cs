using Mapster;
using Microsoft.EntityFrameworkCore;
using Ordering.Data;
using Ordering.Orders.Dtos;
using Ordering.Orders.Exceptions;
using Shared.Contracts.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Orders.Features.GetOrderById
{
	public record GetOrderByIdQuery(Guid Id):IQuery<GetOrderByIdResult>;

	public record GetOrderByIdResult(OrderDto Order);

	public class GetOrderByIdHandler(OrderingDbContext dbContext) : IQueryHandler<GetOrderByIdQuery, GetOrderByIdResult>
	{
		public async Task<GetOrderByIdResult> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
		{
			var order = await dbContext.Orders
								.AsNoTracking()
								.Include(o => o.Items)
								.SingleOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
			if (order is null)
			{
				throw new OrderNotFoundException(request.Id);
			}

			var orderDto = order.Adapt<OrderDto>();

			return new GetOrderByIdResult(orderDto);
		}
	}
}
