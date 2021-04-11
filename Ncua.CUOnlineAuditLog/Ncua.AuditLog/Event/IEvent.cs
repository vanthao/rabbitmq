using System.Collections.Generic;

namespace Ncua.AuditLog.Event
{
	public interface IEvent
	{
		//Correlation id which translates to a global id for any unit of work in the system
		long? CorrelationId { get; set; }

		//Generic event attributes
		IDictionary<int, string> EventAttributes { get; set; }

		//Category id for the event
		int EventTypeId { get; set; }
	}
}