﻿using Ordering.Data;
using Shared.Data;
using Shared.Data.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace Ordering
{
    public static class OrderingModule
    {
        public static IServiceCollection AddOrderingModule(this IServiceCollection services, IConfiguration configuration)
        {
			var connectionString = configuration.GetConnectionString("Database");

			services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
			services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

			services.AddDbContext<OrderingDbContext>((sp, options) =>
			{
				options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
				options.UseNpgsql(connectionString);
			});

			return services;
        }

        public static IApplicationBuilder UseOrderingModule(this IApplicationBuilder app)
        {
			app.UseMigration<OrderingDbContext>();
			return app;
        }
    }
}
