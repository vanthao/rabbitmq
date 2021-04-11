using Ncua.CUOnline.AuditLog.Validators;
using Ncua.CUOnline.AuditLog.EventTypes;
using Ncua.CUOnline.AuditLog.Attributes;
namespace Ncua.CUOnline.AuditLog.EventTypes
{
    /// <summary>
    /// Track event class that inherits from Base Event
    /// </summary>
    public class TrackEvent : BaseEvent<TrackAttribute>
	{
		#region Constructor
		public TrackEvent() : this(new EventValidator<TrackAttribute>())
		{}

		public TrackEvent(IEventValidator<TrackAttribute> eventValidator): base(eventValidator)
		{
			// Set the values required for logging an audit event.
			base._EventType = EventType.Track.ToString();
			base._EventTypeId = (int)EventType.Track;
		}
		#endregion
	}
}
