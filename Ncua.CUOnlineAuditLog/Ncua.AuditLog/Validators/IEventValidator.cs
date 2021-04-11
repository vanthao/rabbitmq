using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ncua.AuditLog.EventTypes;

namespace Ncua.AuditLog.Validators
{
	/// <summary>
	/// Interface for validating events
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IEventValidator<T>
	{
		/// <summary>
		/// Method to perform validations an an event
		/// </summary>
		/// <param name="eventInfo"></param>
		/// <param name="details"></param>
		/// <returns></returns>
		bool IsValidEvent(BaseEvent<T> eventInfo, out StringBuilder details);
	}
}
