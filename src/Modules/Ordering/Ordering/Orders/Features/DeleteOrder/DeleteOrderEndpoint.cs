﻿using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Orders.Features.DeleteOrder
{
	public record DeleteOrderResponse(bool IsSuccess);
	public class DeleteOrderEndpoint : ICarterModule
	{

		public void AddRoutes(IEndpointRouteBuilder app)
		{
			app.MapDelete("/orders/{id}", async (Guid id, ISender sender) =>
			{
				var result = await sender.Send(new DeleteOrderCommand(id));
				var response = result.Adapt<DeleteOrderResponse>();
				return Results.Ok(response);
			})
			.WithName("DeleteOrder")
			.Produces<DeleteOrderResponse>(StatusCodes.Status200OK)
			.ProducesProblem(StatusCodes.Status400BadRequest)
			.ProducesProblem(StatusCodes.Status404NotFound)
			.WithSummary("Delete Order").WithDescription("Delete Order");
		}
	}
}
