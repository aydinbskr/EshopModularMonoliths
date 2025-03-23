using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static MassTransit.Monitoring.Performance.BuiltInCounters;

namespace Basket.Data.Processors
{
	public class OutboxProcessor (IServiceProvider serviceProvider,IBus bus, ILogger<OutboxProcessor> logger)
		: BackgroundService
	{
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while(!stoppingToken.IsCancellationRequested)
			{
				try
				{
					using var scope = serviceProvider.CreateScope();
					var dbcContext = scope.ServiceProvider.GetRequiredService<BasketDbContext>();

					var outboxMessages = await dbcContext.OutboxMessages
						.Where(o => o.ProcessedOn == null)
						.ToListAsync(stoppingToken);


					foreach (var message in outboxMessages)
					{
						var eventType = Type.GetType(message.Type);
						if (eventType == null)
						{
							logger.LogWarning("Could not resolve type: {Type}", message.Type);
							continue;
						}


						var eventMessage = JsonSerializer.Deserialize(message.Content, eventType);
						if (eventMessage == null)
						{
							logger.LogWarning("Could not deserialize message: {Content}", message.Content);

							continue;
						}

						await bus.Publish(eventMessage, stoppingToken);
						message.ProcessedOn = DateTime.UtcNow;

						logger.LogInformation("Successfully processed outbox message with ID:{Id}", message.Id);
					}

					await dbcContext.SaveChangesAsync(stoppingToken);

				}
				catch (Exception ex)
				{

					logger.LogError(ex, "Error processing outbox messages");
				}

				await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
			}
		}
	}
}
