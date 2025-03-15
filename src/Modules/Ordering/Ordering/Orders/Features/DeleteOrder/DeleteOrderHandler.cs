using FluentValidation;
using Ordering.Data;
using Ordering.Orders.Exceptions;
using Shared.Contracts.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Orders.Features.DeleteOrder
{
	public record DeleteOrderCommand(Guid OrderId) : ICommand<DeleteOrderResult>;
	public record DeleteOrderResult(bool IsSuccess);

	public class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
	{
		public DeleteOrderCommandValidator()
		{
			RuleFor(x => x.OrderId).NotEmpty().WithMessage("Order Id is required");
		}
	}

	internal class DeleteOrderHandler(OrderingDbContext dbContext) : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
	{
		public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
		{

			var Order = await dbContext.Orders
			.FindAsync([command.OrderId], cancellationToken: cancellationToken);

			if (Order is null)
			{
				throw new OrderNotFoundException(command.OrderId);

			}

			dbContext.Orders.Remove(Order);
			await dbContext.SaveChangesAsync(cancellationToken);
			return new DeleteOrderResult(true);
		}
	}
}
