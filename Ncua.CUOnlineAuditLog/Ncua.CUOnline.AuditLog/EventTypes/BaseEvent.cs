using Ncua.CUOnline.AuditLog.Validators;
using System;
using System.Collections.Generic;
using System.Diagnostics;
namespace Ncua.CUOnline.AuditLog.EventTypes
{
    public abstract class BaseEvent<T> : IBaseEvent<T>
	{
		#region Members
		#region IBaseEvent<T> Members
		long? _CorrelationId;
		string _Message;
		TraceEventType? _Severity;
		IDictionary<T, string> _EventAttributes;
		#endregion

		IEventValidator<T> _EventValidator;
		//LogWriter _LogWriter;

		internal string _EventType;
		internal int _EventTypeId;

		//MessagePriority _DefaultPriority;
		TraceEventType _DefaultSeverity;
		#endregion

		#region IBaseEvent<T> Properties
		public long? CorrelationId
		{
			get { return _CorrelationId; }
			set { _CorrelationId = value; }
		}

		public IDictionary<T, string> EventAttributes
		{
			get { return _EventAttributes; }
			set { _EventAttributes = value; }
		}

		public string Message
		{
			get { return _Message; }
			set { _Message = value; }
		}

		
		public System.Diagnostics.TraceEventType? Severity
		{
			get { return _Severity; }
			set { _Severity = value; }
		} 
		#endregion

		#region IBaseEvent<T> Methods
		
		public void Log()
		{
			//Create a transaction scope with a suppress option so that this logic does not participate in ambient transaction
			try
			{

				;
			}
			catch(Exception ex)
            {
				;

			}
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Default class constructor
		/// </summary>
		public BaseEvent()
			: this(new EventValidator<T>())
		{ }

		/// <summary>
		/// Initializes class with a specific validator
		/// </summary>
		public BaseEvent(IEventValidator<T> eventValidator)
		{
			// Validate the arguments which are required for logging
			//this.ValidateLoggingDefaults();

			// Associate the EventValidtor
			this._EventValidator = eventValidator;

			// Initialize generic Event attribute dictionary
			this._EventAttributes = new Dictionary<T, string>();
		}
		#endregion

		#region Methods
		//private void ValidateLoggingDefaults()
		//{
		//	//Initialize default values for logging event
		//	string defaultSeverity = FrameworkConfig.ConfigurationManager.GetSettingValue(EventShared.Predefined.AppSettingKeys.DefaultSeverityKey);
		//	string defaultPriority = FrameworkConfig.ConfigurationManager.GetSettingValue(EventShared.Predefined.AppSettingKeys.DefaultPriorityKey);

		//	//Check if default severity is supplied
		//	if (String.IsNullOrEmpty(defaultSeverity))
		//		throw new ConfigurationErrorsException(String.Format("{0} does not have a {1} key configured to log events",
		//			AppDomain.CurrentDomain.FriendlyName, EventShared.Predefined.AppSettingKeys.DefaultSeverityKey));

		//	//Check if default severity defined is contained in the TraceEventType Enum
		//	if (!Enum.IsDefined(typeof(TraceEventType), defaultSeverity))
		//		throw new ArgumentException("Default severity level does not exist in the TraceEventType Enum", defaultSeverity);

		//	//Check if default priority is supplied
		//	if (String.IsNullOrEmpty(defaultPriority))
		//		throw new ConfigurationErrorsException(String.Format("{0} does not have a {1} key configured to log events",
		//			AppDomain.CurrentDomain.FriendlyName, EventShared.Predefined.AppSettingKeys.DefaultPriorityKey));

		//	//Check if default priority defined is contained in the MessagePriority Enum
		//	if (!Enum.IsDefined(typeof(MessagePriority), defaultPriority))
		//		throw new ArgumentException("Default priority level does not exist in the MessagePriority Enum", defaultPriority);

		//	//set default severity for use by event
		//	this._DefaultSeverity = (TraceEventType)Enum.Parse(typeof(TraceEventType), defaultSeverity, true);
			
		//	//set default priority for use by event     
		//	this._DefaultPriority = (MessagePriority)Enum.Parse(typeof(MessagePriority), defaultPriority, true);
		//}
		#endregion
	}
}
