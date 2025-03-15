using Ordering.Orders.Models;
using Shared.DDD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Orders.Events
{
	public record OrderCreatedEvent(Order order):IDomainEvent;
}
