
using Ncua.AuditLog.Attributes;
using Ncua.AuditLog.Validators;
namespace Ncua.AuditLog.EventTypes
{
    /// <summary>
    /// Trace event class that inherits from Base Event
    /// </summary>
    public class TraceEvent : BaseEvent<TraceAttribute>
	{
		#region Constructor
		public TraceEvent() : this(new EventValidator<TraceAttribute>())
		{}

		public TraceEvent(IEventValidator<TraceAttribute> eventValidator): base(eventValidator)
		{
			// Set the values required for logging an audit event.
			base._EventType = EventType.Trace.ToString();
			base._EventTypeId = (int)EventType.Trace;
		}
		#endregion
	}
}
