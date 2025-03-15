﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Orders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Data.Configurations
{
	public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
	{
		public void Configure(EntityTypeBuilder<OrderItem> builder)
		{
			builder.HasKey(e => e.Id);
			builder.Property(oi => oi.ProductId).IsRequired();
			builder.Property(oi => oi.Quantity).IsRequired();
			builder.Property(oi => oi.Price).IsRequired();
		}
	}
}
