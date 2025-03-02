using Catalog.Products.Models;
using MassTransit;
using Shared.Messaging.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Catalog.Products.EventHandlers
{
	public class ProductPriceChangedEventHandler(IBus bus,ILogger<ProductPriceChangedEventHandler> logger)
		: INotificationHandler<ProductPriceChengedEvent>
	{
		public async Task Handle(ProductPriceChengedEvent notification, CancellationToken cancellationToken)
		{
			logger.LogInformation("Domain event handled:{DomainEvent}", notification.GetType().Name);


			var integrationEvent = new ProductPriceChangedIntegrationEvent
			{
				ProductId = notification.Product.Id,
				Name = notification.Product.Name,
				Category = notification.Product.Category,
				Description = notification.Product.Description,
				ImageFile = notification.Product.ImageFile,
				Price = notification.Product.Price //set updated product price
			};


			await bus.Publish(integrationEvent, cancellationToken);
		}
	}
}
