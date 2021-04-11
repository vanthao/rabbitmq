using System;
using System.Collections.Generic;
using System.Text;

namespace Ncua.AuditLog.Event
{
	public interface IServiceMessage
	{
		/// <summary>
		/// Contains the name of the source application sending the message.
		/// </summary>
		string Source { get; set; }

		/// <summary>
		/// Contains the name of the Target service of the message.
		/// </summary>
		string Target { get; set; }

		/// <summary>
		/// Gets or sets the message priority of this service message.
		/// </summary>
		int Priority { get; set; }

		/// <summary>
		/// The GlobalId associated with the service message
		/// </summary>
		long GlobalId { get; set; }

		/// <summary>
		/// A value representing the current state of the message. Used to drive 
		/// processing workflow.
		/// </summary>
		string State { get; set; }

		/// <summary>
		/// A value representing proper lock key for each service message type,
		/// this value can be used for serializing message processing.
		/// </summary>
		string LockKey { get; set; }

		/// <summary>
		/// The lock key used by individual service to lock and unlock messages.
		/// </summary>
		string InternalLockKey { get; set; }

		/// <summary>
		/// Gets a value indicating if the service message supplied a value for LockKey.
		/// </summary>
		bool RequiresLock { get; }

		/// <summary>
		/// Gets a value indicating if the service utilized an internal lock on message.
		/// </summary>
		bool RequiresInternalLock { get; }

		/// <summary>
		/// Contains the address of the endpoint from which the message was received.
		/// </summary>
		string ReceivedFromAddress { get; set; }

		/// <summary>
		/// Contains the address of the endpoint to which the reply should be sent.
		/// </summary>
		string ReplyToAddress { get; set; }
	}
}
