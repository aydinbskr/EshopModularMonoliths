using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Messaging.Extensions
{
	public static class MassTransitExtensions
	{

		public static IServiceCollection AddMassTransitWithAssemblies
		(this IServiceCollection services, params Assembly[] assemblies)
		{
			services.AddMassTransit(config =>
			{
				config.SetKebabCaseEndpointNameFormatter();

				config.SetInMemorySagaRepositoryProvider();
				config.AddConsumers(assemblies); config.AddSagaStateMachines(assemblies);
				config.AddSagas(assemblies);
				config.AddActivities(assemblies);

				config.UsingInMemory((context, configurator) =>
				{
					configurator.ConfigureEndpoints(context);
				});

			});
			return services;

		}


	}
}
