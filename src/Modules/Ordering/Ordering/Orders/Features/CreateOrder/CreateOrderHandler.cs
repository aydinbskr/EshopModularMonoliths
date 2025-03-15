using FluentValidation;
using Ordering.Data;
using Ordering.Orders.Dtos;
using Ordering.Orders.Models;
using Ordering.Orders.ValueObjects;
using Shared.Contracts.CQRS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Orders.Features.CreateOrder
{
	public record CreateOrderCommand(OrderDto Order)
		: ICommand<CreateOrderResult>;

	public record CreateOrderResult(Guid Id);


	public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
	{
		public CreateOrderCommandValidator()
		{
			RuleFor(x => x.Order.OrderName).NotEmpty().WithMessage("OrderName is required");
			
		}
	}

	public class CreateOrderHandler
		(OrderingDbContext dbContext)
		: ICommandHandler<CreateOrderCommand, CreateOrderResult>
	{
		public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
		{
			//actual logic
			var order = CreateNewOrder(command.Order);

			dbContext.Orders.Add(order);
			await dbContext.SaveChangesAsync(cancellationToken);

			return new CreateOrderResult(order.Id);
		}
		private Order CreateNewOrder(OrderDto orderDto)
		{
			var shippingAddress = Address.Of(orderDto.ShippingAddress.FirstName, orderDto.ShippingAddress.LastName, orderDto.ShippingAddress.EmailAddress, orderDto.ShippingAddress.AddressLine, orderDto.ShippingAddress.Country, orderDto.ShippingAddress.State, orderDto.ShippingAddress.ZipCode);
			var billingAddress = Address.Of(orderDto.BillingAddress.FirstName, orderDto.BillingAddress.LastName, orderDto.BillingAddress.EmailAddress, orderDto.BillingAddress.AddressLine, orderDto.BillingAddress.Country, orderDto.BillingAddress.State, orderDto.BillingAddress.ZipCode);
			
			
			var newOrder = Order.Create(
			Guid.NewGuid(),
			orderDto.CustomerId,
			orderDto.OrderName,
			shippingAddress,
			billingAddress,
			Payment.Of(orderDto.Payment.CardName,orderDto.Payment.CardNumber,orderDto.Payment.Expiration, orderDto.Payment.Cvv,orderDto.Payment.PaymentMethod));

			orderDto.Items.ForEach(item =>
			{
				newOrder.Add(item.ProductId, item.Quantity, item.Price);
			});

			return newOrder;
		}
	}
}
