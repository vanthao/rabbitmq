using Ncua.AuditLog.Attributes;
using Ncua.AuditLog.Validators;
namespace Ncua.AuditLog.EventTypes
{
    /// <summary>
    /// Audit Event class that inherits from Base Event
    /// </summary>
    public class AuditEvent : BaseEvent<AuditAttribute>
	{

		#region Constructor
		public AuditEvent() : this(new EventValidator<AuditAttribute>())
		{
			
		}

		public AuditEvent(IEventValidator<AuditAttribute> eventValidator): base(eventValidator)
		{
			// Set the values required for logging an audit event.
			base._EventType = EventType.Audit.ToString();
			base._EventTypeId = (int)EventType.Audit;
		}
		#endregion

		#region Methods
		public static AuditEvent CreateAuditEvent(string operation, string source, string target)
		{
			//call default method
			return CreateAuditEvent(operation, source, target, null);
		}

		public static AuditEvent CreateAuditEvent(string operation, string source, string target, string message)
		{
			//call default method
			AuditEvent audit = new AuditEvent();
			audit.EventAttributes.Add(AuditAttribute.Operation, operation);
			audit.EventAttributes.Add(AuditAttribute.Source, source);
			audit.EventAttributes.Add(AuditAttribute.Target, target);
			audit.EventAttributes.Add(AuditAttribute.Message, message);
			return audit;
		}
		#endregion
	}
}
