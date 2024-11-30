using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Products.EventHandlers
{
	public class ProductPriceChangedEventHandler(ILogger<ProductPriceChangedEventHandler> logger)
		: INotificationHandler<ProductPriceChengedEvent>
	{
		public Task Handle(ProductPriceChengedEvent notification, CancellationToken cancellationToken)
		{
			logger.LogInformation("Domain event handled:{DomainEvent}", notification.GetType().Name);
			return Task.CompletedTask;
		}
	}
}
