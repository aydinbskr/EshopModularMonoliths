using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Ordering.Orders.Dtos;
using Shared.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Orders.Features.GetOrders
{
	public record GetOrdersRequest(PaginationRequest PaginationRequest);
	public record GetOrdersResponse(PaginatedResult<OrderDto> Orders);
	public class GetOrdersEndpoint : ICarterModule
	{
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapGet("/orders", async ([AsParameters] PaginationRequest request, ISender sender) =>
			{
				var result = await sender.Send(new GetOrdersQuery(request));
				var response = result.Adapt<GetOrdersResponse>();
				return Results.Ok(response);
			})
			.WithName("GetOrders")
			.Produces<GetOrdersResponse>(StatusCodes.Status200OK)
			.ProducesProblem(StatusCodes.Status400BadRequest)
			.WithSummary("Get Orders")
			.WithDescription("Get Orders");
		}
	}
}
