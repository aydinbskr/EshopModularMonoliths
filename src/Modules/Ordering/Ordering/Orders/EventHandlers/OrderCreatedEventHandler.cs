using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Orders.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Orders.EventHandlers
{

	public class OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger)
	: INotificationHandler<OrderCreatedEvent>
	{
		public Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
		{
			logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
			return Task.CompletedTask;
		}
	}
}
