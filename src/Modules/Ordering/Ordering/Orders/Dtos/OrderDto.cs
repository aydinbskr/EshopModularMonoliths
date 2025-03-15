using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Orders.Dtos
{
	public record OrderDto(Guid Id, Guid CustomerId, string OrderName,
		AddressDto ShippingAddress, AddressDto BillingAddress, PaymentDto Payment,
		List<OrderItemDto> Items);
}
