using Microsoft.EntityFrameworkCore;
using Ordering.Orders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Data
{
	public class OrderingDbContext : DbContext
	{
		public OrderingDbContext(DbContextOptions<OrderingDbContext> options) : base(options)
		{
		}

		public DbSet<Order> Orders => Set<Order>();
		public DbSet<OrderItem> OrderItems => Set<OrderItem>();

		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.HasDefaultSchema("ordering");
			builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
			base.OnModelCreating(builder);
		}
	}
}
