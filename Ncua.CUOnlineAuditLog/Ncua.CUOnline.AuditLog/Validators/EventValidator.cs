using Ncua.CUOnline.AuditLog.EventTypes;
using System;
using System.Text;
namespace Ncua.CUOnline.AuditLog.Validators
{
    internal class EventValidator<T> : IEventValidator<T>
	{
		#region IEventValidator<T> Methods
		/// <summary>
		/// Method to perform basic validations on event attributes
		/// </summary>
		/// <param name="eventInfo">The event object that is being validated</param>
		/// <param name="details">The out parameter that contains the validation details</param>
		/// <returns></returns>
		public bool IsValidEvent(EventTypes.BaseEvent<T> eventInfo, out StringBuilder details)
		{
			//internal flag to monitor processing
			bool _validated = false;

			//String builder to hold validation details
			details = new StringBuilder();
			
			//TODO:Get more specifications for validations that might need to take place here
			//do basic validations for missing attributes and empty attributes
			if (eventInfo.EventAttributes.Count == 0)
				details.Append(String.Format("{0} Event created by {1} has no attributes", eventInfo._EventType, AppDomain.CurrentDomain.FriendlyName));
			else
				_validated = true;

			return _validated;
		}

        #endregion

        #region Constructor
        public EventValidator() {}
		#endregion
	}
}
