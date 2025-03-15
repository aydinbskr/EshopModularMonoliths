using Mapster;
using Microsoft.EntityFrameworkCore;
using Ordering.Data;
using Ordering.Orders.Dtos;
using Shared.Contracts.CQRS;
using Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Orders.Features.GetOrders
{
	public record GetOrdersQuery(PaginationRequest Request) : IQuery<GetOrdersResult>;
	public record GetOrdersResult(PaginatedResult<OrderDto> Orders);

	internal class GetOrdersHandler(OrderingDbContext dbContext)
		: IQueryHandler<GetOrdersQuery, GetOrdersResult>
	{
		public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
		{
			var pageIndex = query.Request.PageIndex;
			var pageSize = query.Request.PageSize;

			var totalCount = await dbContext.Orders.LongCountAsync(cancellationToken);

			var orders = await dbContext.Orders
			.AsNoTracking()
			.Include(p => p.Items)
			.OrderBy(p => p.OrderName)
			.Skip(pageSize * pageIndex)
			.Take(pageSize)
			.ToListAsync(cancellationToken);

			//mapping Order entity to Orderdto
			var orderDtos = orders.Adapt<List<OrderDto>>();

			return new GetOrdersResult(
				new PaginatedResult<OrderDto>(pageIndex, pageSize, totalCount, orderDtos)
				);

		}
	}
}
