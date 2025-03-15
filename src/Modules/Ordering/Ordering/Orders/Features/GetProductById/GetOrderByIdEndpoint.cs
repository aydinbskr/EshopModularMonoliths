using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Ordering.Orders.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Orders.Features.GetOrderById
{
	public record GetOrderByIdResponse(OrderDto Order);
	public class GetOrderByIdEndpoint : ICarterModule
	{
		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapGet("/orders/{id}", async (Guid id, ISender sender) =>
			{
				var result = await sender.Send(new GetOrderByIdQuery(id));
				var response = result.Adapt<GetOrderByIdResponse>();
				return Results.Ok(response);
			})
			.WithName("GetOrderById")
			.Produces<GetOrderByIdResponse>(StatusCodes.Status200OK)
			.ProducesProblem(StatusCodes.Status400BadRequest)
			.WithSummary("Get Order By Id")
			.WithDescription("Get Order By Id");
		}
	}
}
