using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Ordering.Orders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Data.Configurations
{
	public class OrderConfiguration : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.HasKey(e => e.Id);
			builder.Property(o => o.CustomerId);

			builder.HasIndex(e => e.OrderName).IsUnique();

			builder.Property(o => o.OrderName).IsRequired().HasMaxLength(100);

			builder.HasMany(s => s.Items).WithOne().HasForeignKey(si => si.OrderId);

			builder.ComplexProperty(o => o.ShippingAddress, addressbuilder =>
			{
				addressbuilder.Property(a => a.FirstName).HasMaxLength(50).IsRequired();
				addressbuilder.Property(a => a.LastName).HasMaxLength(50).IsRequired();
				addressbuilder.Property(a => a.EmailAddress).HasMaxLength(50).IsRequired();
				addressbuilder.Property(a => a.AddressLine).HasMaxLength(180).IsRequired();
				addressbuilder.Property(a => a.Country).HasMaxLength(50).IsRequired();
				addressbuilder.Property(a => a.State).HasMaxLength(50).IsRequired();
				addressbuilder.Property(a => a.ZipCode).HasMaxLength(50).IsRequired();
			});

			builder.ComplexProperty(o => o.BillingAddress, addressbuilder =>
			{
				addressbuilder.Property(a => a.FirstName).HasMaxLength(50).IsRequired();
				addressbuilder.Property(a => a.LastName).HasMaxLength(50).IsRequired();
				addressbuilder.Property(a => a.EmailAddress).HasMaxLength(50).IsRequired();
				addressbuilder.Property(a => a.AddressLine).HasMaxLength(180).IsRequired();
				addressbuilder.Property(a => a.Country).HasMaxLength(50).IsRequired();
				addressbuilder.Property(a => a.State).HasMaxLength(50).IsRequired();
				addressbuilder.Property(a => a.ZipCode).HasMaxLength(50).IsRequired();
			});

			builder.ComplexProperty(o => o.Payment, paymentbuilder =>
			{
				paymentbuilder.Property(a => a.CardName).HasMaxLength(50).IsRequired();
				paymentbuilder.Property(a => a.CardNumber).HasMaxLength(24).IsRequired();
				paymentbuilder.Property(a => a.Expiration).HasMaxLength(10).IsRequired();
				paymentbuilder.Property(a => a.CVV).HasMaxLength(3).IsRequired();
				paymentbuilder.Property(a => a.PaymentMethod);
				
			});
		}
	}
}
