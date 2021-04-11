using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ncua.AuditLog.Event
{
	/// <summary>
	/// Event entry class to hold event information being passed onto the Microsoft Ent Lib Logging block.
	/// Extended from the MS Ent Lib Logging block native Log Entry object
	/// </summary>
	[Serializable]
	public class Event : LogEntry, IServiceMessage, IEvent
	{
		#region IServiceMessage Properties
		/// <summary>
		/// Contains the name of the source application sending the message.
		/// </summary>
		public string Source { get; set; }

		/// <summary>
		/// Contains the name of the Target service of the message.
		/// </summary>
		public string Target { get; set; }

		/// <summary>
		/// The GlobalId associated with the service message
		/// </summary>
		public long GlobalId { get; set; }

		/// <summary>
		/// A value representing the current state of the message. Used to drive 
		/// processing workflow.
		/// </summary>
		public string State { get; set; }

		/// <summary>
		/// A value representing proper lock key for each service message type,
		/// this value can be used for serializing message processing.
		/// </summary>
		public string LockKey { get; set; }

		/// <summary>
		/// The lock key used by individual service to lock and unlock messages.
		/// </summary>
		public string InternalLockKey { get; set; }

		/// <summary>
		/// Gets a value indicating if the service message supplied a value for LockKey.
		/// </summary>
		public bool RequiresLock
		{
			get { return String.IsNullOrWhiteSpace(this.LockKey).Equals(false); }
		}

		/// <summary>
		/// Gets a value indicating if the service utilized an internal lock on message.
		/// </summary>
		public bool RequiresInternalLock
		{
			get { return String.IsNullOrWhiteSpace(this.InternalLockKey).Equals(false); }
		}

		/// <summary>
		/// Contains the address of the endpoint from which the message was received.
		/// </summary>
		public string ReceivedFromAddress { get; set; }

		/// <summary>
		/// Contains the address of the endpoint to which the reply should be sent.
		/// </summary>
		public string ReplyToAddress { get; set; }

		#endregion

		#region IEvent Properties
		private long? _CorrelationId;
		private int _EventTypeId;
		private IDictionary<int, string> _EventAttributes;

		public long? CorrelationId
		{
			get { return _CorrelationId; }
			set { _CorrelationId = value; }
		}

		public IDictionary<int, string> EventAttributes
		{
			get { return _EventAttributes; }
			set { _EventAttributes = value; }
		}

		public int EventTypeId
		{
			get { return _EventTypeId; }
			set { _EventTypeId = value; }
		}

       #endregion

        #region Constructor
        public Event() : base()
		{
			this._EventAttributes = new Dictionary<int, string>();
		}
		#endregion
	}
}
