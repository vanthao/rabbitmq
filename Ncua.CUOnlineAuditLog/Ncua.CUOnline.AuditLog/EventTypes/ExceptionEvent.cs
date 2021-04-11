using System;
using Ncua.CUOnline.AuditLog.Attributes;
using Ncua.CUOnline.AuditLog.Validators;
namespace Ncua.CUOnline.AuditLog.EventTypes
{
	/// <summary>
	/// Exception event class that inherits from Base Event
	/// </summary>
	public class ExceptionEvent: BaseEvent<ExceptionAttribute>
	{
		#region Constructor
		public ExceptionEvent() : this(new EventValidator<ExceptionAttribute>())
		{}

		public ExceptionEvent(IEventValidator<ExceptionAttribute> eventValidator): base(eventValidator)
		{
			// Set the values required for logging an audit event.
			base._EventType = EventType.Exception.ToString();
			base._EventTypeId = (int)EventType.Exception;
		}
		#endregion

		#region Public Methods

		/// <summary>
		/// This method takes a system generated exception and returns
		///  an Exception event wrapper for use by the client
		/// </summary>
		/// <param name="ex">The originating exception</param>
		/// <returns>ExceptionEvent class</returns>
		public static ExceptionEvent CreateException(Exception ex)
		{
			// call the overloaded method to create exception
			return CreateException(ex, ex.GetType().ToString());
		}

		/// <summary>
		/// This method takes a system generated exception and an Exception type
		/// and returns an Exception event wrapper for use by the client
		/// </summary>
		/// <param name="ex">The originating exception</param>
		/// <param name="excType">The functional type of this exception</param>
		/// <returns>ExceptionEvent class</returns>
		public static ExceptionEvent CreateException(Exception ex, string excType)
		{
			// call another overloaded method
			return CreateException(ex, excType, null);
		}

		/// <summary>
		/// This method takes a system generated exception and an Exception type (Account, Client, Group, Product, etc.)   
		/// and returns an Exception Event wrapper for use by the client
		/// </summary>
		/// <param name="ex">The originating exception</param>
		/// <param name="excType">The functional type of this exception</param>
		/// <param name="id">The global id related to the unit of work that created this exception</param>
		/// <returns>Exception Event</returns>
		public static ExceptionEvent CreateException(Exception ex, string excType, Nullable<long> id)
		{
			return CreateException(ex, excType, id, null);
		}

		/// <summary>
		/// This method takes a system generated exception and an Exception type (Account, Client, Group, Product, etc.)   
		/// and returns an Exception Event wrapper for use by the client
		/// </summary>
		/// <param name="ex">The originating exception</param>
		/// <param name="excType">The functional type of this exception</param>
		/// <param name="id">The global id related to the unit of work that created this exception</param>
		/// <param name="generalMsg">The general message related to this exception event. Any general identifying information will suffice here</param>
		/// <returns>Exception Event</returns>
		public static ExceptionEvent CreateException(Exception ex, string excType, Nullable<long> id, string generalMsg)
		{
			// Instantiate exception event, populate the required properties and return back to the client
			ExceptionEvent exceptionEvent = new ExceptionEvent();

			exceptionEvent.CorrelationId = id;
			exceptionEvent.Message = generalMsg;

			// Set the detail level attributes
			exceptionEvent.EventAttributes[ExceptionAttribute.ExceptionType] = excType;
			exceptionEvent.EventAttributes[ExceptionAttribute.Message] = String.Format("{0}", ex.Message);
			exceptionEvent.EventAttributes[ExceptionAttribute.Source] = String.Format("{0}", ex.Source);
			exceptionEvent.EventAttributes[ExceptionAttribute.StackTrace] = String.Format("{0}", ex.StackTrace);
			exceptionEvent.EventAttributes[ExceptionAttribute.TargetSite] = String.Format("{0}", ex.TargetSite);

			return exceptionEvent;
		}
		#endregion
	}
}
