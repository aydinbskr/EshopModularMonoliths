﻿using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shared.Behaviours;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Extensions
{

	public static class MediatRExtentions
	{
		public static IServiceCollection AddMediatRWithAssemblies
				(this IServiceCollection services, params Assembly[] assemblies)
		{
			services.AddMediatR(config =>
			{
				config.RegisterServicesFromAssemblies(assemblies);
				config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
				config.AddOpenBehavior(typeof(LoggingBehaviour<,>));

			});

			services.AddValidatorsFromAssemblies(assemblies);
			return services;
		}

	}
}
