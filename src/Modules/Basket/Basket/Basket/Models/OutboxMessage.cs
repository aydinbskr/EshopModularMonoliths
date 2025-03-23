using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.Basket.Models
{

	public class OutboxMessage : Entity<Guid>
	{
		public string Type { get; set; } = default!;
		public string Content { get; set; } = default!;
		public DateTime OccuredOn { get; set; } = default!;
		public DateTime? ProcessedOn { get; set; } = default!;

	}
}
