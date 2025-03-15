using Shared.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Orders.Exceptions
{
	public class OrderNotFoundException : NotFoundException
	{
		public OrderNotFoundException(Guid id)
		: base("Order", id)
		{
		}
	}
}
