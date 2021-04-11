using System.Collections.Generic;
using System.Diagnostics;

namespace Ncua.AuditLog.EventTypes
{
    interface IBaseEvent<T>
	{
		#region Properties
		/// <summary>
		/// Global Id which ties together the unit of work that
		/// comes into the SOA
		/// </summary>
		long? CorrelationId { get; set; }

		/// <summary>
		/// Generic Dictionary collection to store the event attributes
		/// </summary>
		IDictionary<T, string> EventAttributes { get; set; }

		/// <summary>
		/// Generic message related to the Event
		/// </summary>
		string Message { get; set; }


		/// <summary>
		/// Severity of the Message
		/// </summary>
		TraceEventType? Severity { get; set; }
		#endregion

		#region Methods
		/// <summary>
		/// This will log the event
		/// </summary>
		void Log();
		#endregion
	}
}
