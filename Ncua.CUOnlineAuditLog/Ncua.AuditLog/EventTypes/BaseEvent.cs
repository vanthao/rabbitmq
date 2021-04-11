using Ncua.AuditLog.Validators;
using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using RabbitMQ.Client;
using System.Configuration;
using Newtonsoft.Json;
using System.Linq;
using Ncua.Logging.Entity;

namespace Ncua.AuditLog.EventTypes
{
    public abstract class BaseEvent<T> : IBaseEvent<T>
	{
		#region Members
		private IModel m_Channel;
		private IConnection m_Connection;
		private string m_ExchangeName;
		private string m_QueueName;
		private string m_RoutingKey;

		private string m_ExchangeName_DeadLetter;
		private string m_QueueName_DeadLetter;
		private string m_RoutingKey_DeadLetter;

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
        private object _Priority;
        private int _DefaultPriority;
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
		 private void InitConfig()
        {
			m_ExchangeName = ConfigurationManager.AppSettings["RabbitMQ_SystemLog_ExchangeName"].ToString();
			m_QueueName = ConfigurationManager.AppSettings["RabbitMQ_SystemLog_QueueName"].ToString();
			m_RoutingKey = ConfigurationManager.AppSettings["RabbitMQ_SystemLog_RoutingKey"].ToString();

			m_ExchangeName_DeadLetter = ConfigurationManager.AppSettings["RabbitMQ_SystemLog_DeadLetter_ExchangeName"].ToString();
			m_QueueName_DeadLetter = ConfigurationManager.AppSettings["RabbitMQ_SystemLog_DeadLetter_QueueName"].ToString();
			m_RoutingKey_DeadLetter = ConfigurationManager.AppSettings["RabbitMQ_SystemLog_DeadLetter_RoutingKey"].ToString();

			var connectionFactory = new ConnectionFactory()
			{
				HostName = ConfigurationManager.AppSettings["RabbitMQ_HostName"].ToString(),
				Port = Int32.Parse(ConfigurationManager.AppSettings["RabbitMQ_HostPort"].ToString()),
				UserName = ConfigurationManager.AppSettings["RabbitMQ_UserName"].ToString(),
				Password = ConfigurationManager.AppSettings["RabbitMQ_Password"].ToString()
			};

			m_Connection = connectionFactory.CreateConnection();
			m_Channel = m_Connection.CreateModel();

			// DEAD LETTER QUEUE
			IDictionary<string, object> arguments = new Dictionary<string, object>()
						{
						   {"x-dead-letter-exchange", m_ExchangeName_DeadLetter}
								};
			//m_Channel.ExchangeDeclare(m_ExchangeName_DeadLetter, ExchangeType.Fanout, true, false, arguments);
			//m_Channel.QueueDeclare(m_QueueName_DeadLetter, true, false, false, arguments);
			//m_Channel.QueueBind(m_QueueName_DeadLetter, m_ExchangeName_DeadLetter, m_RoutingKey_DeadLetter, arguments);

			// LOG QUEUE
			m_Channel.ExchangeDeclare(m_ExchangeName, ExchangeType.Fanout, true, false,null );
			m_Channel.QueueDeclare(m_QueueName, true, false, false, null);
			m_Channel.QueueBind(m_QueueName, m_ExchangeName, m_RoutingKey, null);
		}
		public BaseEvent() : this(new EventValidator<T>())
        {
			InitConfig();
		}

		public void Log()
		{
			//Initialize and populate event message object
			var eventMsg = new Ncua.Logging.Entity.Event();
			eventMsg.AppDomainName = "Ncua";
			eventMsg.MachineName = "PC";
			eventMsg.CorrelationId = 434343;
			eventMsg.CreateId = "434343";
			eventMsg.UpdateId = "43455";
			eventMsg.EventTypeId = 101;
			eventMsg.CreateDate = DateTime.Now;
			eventMsg.UpdateDate = DateTime.Now;
			eventMsg.GenerateDate = DateTime.Now;
			eventMsg.Severity = "Information";
			//Add category to EventMessage which dictates the the log sink the event message goes to
			//eventMsg.Categories.Add(this._EventType);

			//Global Id in the system which ties different events for the same unit of work/execution together
			//eventMsg.CorrelationId = this._CorrelationId;

			//Convert Dictionary<enum, string> to a Dictionary<int, string> so we can send actual attribute id's 
			//and values for straight insertions at endpoints with minimal validations
			eventMsg.EventDetails = new List<EventDetail>();
			foreach (var attribute in this._EventAttributes)
			{
				var evtDetail = new EventDetail()
				{
					Value = attribute.Value,
					EventAttributeId = (int)Enum.Parse(typeof(T), attribute.Key.ToString(), true),
					CreateDate = DateTime.Now,
					CreateId = "4353545",
					UpdateDate = DateTime.Now,
					UpdateId = "543545",

				};
				eventMsg.EventDetails.Add(evtDetail);
			}
			//eventMsg.EventAttributes =
			//	this._EventAttributes.ToDictionary(kvp => (int)Enum.Parse(typeof(T), kvp.Key.ToString(), true), kvp => kvp.Value);

			//The event category id of the event (Audit, Exception, Trace, etc)
			eventMsg.EventTypeId = this._EventTypeId;

			//The global id for the underlying service framework
			//eventMsg.GlobalId = null != this._CorrelationId ? (long)this._CorrelationId : 0;

			//Any generic informational message in a string format that might need to be included with the event
			eventMsg.Message = this._Message;

			//The priority of the event. Default is used if none is passed in.
			eventMsg.Priority = this._Priority == null ? (int)this._DefaultPriority : (int)this._Priority;

			//The severity of the event. Default is used if none is passed in.
			//eventMsg.Severity = this._Severity == null ? this._DefaultSeverity : (TraceEventType)this._Severity;

			//The application name creating this event
			//eventMsg.Source = ConfigurationManager.AppSettings.Get("AppName");
			var msg = new CustomMessage();
			msg.Message = "this is a test.";
			msg.MessageType = new string[]{"urn:test"};
			string jsonMessage = JsonConvert.SerializeObject(msg);
			LogEvent(jsonMessage);
		}

		public void LogEvent(string jsonMessage)
		{
			if (m_Channel == null)
				InitConfig();
			byte[] messageBuffer = Encoding.UTF8.GetBytes(jsonMessage);

			IBasicProperties basicProperties = m_Channel.CreateBasicProperties();
			basicProperties.DeliveryMode = 2;  // Persistent
			m_Channel.BasicPublish(m_ExchangeName, m_RoutingKey, false, basicProperties, messageBuffer);
			m_Channel.Close();
			m_Connection.Close();
		}
		#endregion

		#region Constructor
		/// <summary>
		/// Default class constructor
		/// </summary>


		/// <summary>
		/// Initializes class with a specific validator
		/// </summary>
		public BaseEvent(IEventValidator<T> eventValidator)
		{
			// Associate the EventValidtor
			this._EventValidator = eventValidator;

			// Initialize generic Event attribute dictionary
			this._EventAttributes = new Dictionary<T, string>();
		}
		#endregion
	}
}
