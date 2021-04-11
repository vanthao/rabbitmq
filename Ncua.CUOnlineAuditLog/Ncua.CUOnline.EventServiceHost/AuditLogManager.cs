using Ncua.Logging.DAL;
using Ncua.Logging.Entity;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading;

namespace Ncua.CUOnline.EventServiceHost
{
   public class AuditLogManager:IDisposable
    {
        private IConnection m_Connection;
        private IModel m_Channel;

        private string m_ExchangeName;
        private string m_QueueName;
        private string m_RoutingKey;
        private string m_HostName;
        private int m_Port;
        private int m_SubscriptionWaitTime;
        private string m_UserName;
        private string m_Password;

        private string m_ExchangeName_DeadLetter;
        private string m_QueueName_DeadLetter;
        private string m_RoutingKey_DeadLetter;

        public EventingBasicConsumer m_Consumer { get; private set; }

        public AuditLogManager()
        {
            m_ExchangeName = ConfigurationManager.AppSettings["RabbitMQ_SystemLog_ExchangeName"].ToString();
            m_QueueName = ConfigurationManager.AppSettings["RabbitMQ_SystemLog_QueueName"].ToString();
            m_RoutingKey = ConfigurationManager.AppSettings["RabbitMQ_SystemLog_RoutingKey"].ToString();

            m_ExchangeName_DeadLetter = ConfigurationManager.AppSettings["RabbitMQ_SystemLog_DeadLetter_ExchangeName"].ToString();
            m_QueueName_DeadLetter = ConfigurationManager.AppSettings["RabbitMQ_SystemLog_DeadLetter_QueueName"].ToString();
            m_RoutingKey_DeadLetter = ConfigurationManager.AppSettings["RabbitMQ_SystemLog_DeadLetter_RoutingKey"].ToString();

            m_HostName = ConfigurationManager.AppSettings["RabbitMQ_HostName"].ToString();
            m_Port = Int32.Parse(ConfigurationManager.AppSettings["RabbitMQ_HostPort"].ToString());
            if (!Int32.TryParse(ConfigurationManager.AppSettings["SystemLogQueueServiceSubscriptionWaitTime"].ToString(), out m_SubscriptionWaitTime))
            {
                m_SubscriptionWaitTime = 2000;
            }
            m_UserName = ConfigurationManager.AppSettings["RabbitMQ_UserName"].ToString();
            m_Password = ConfigurationManager.AppSettings["RabbitMQ_Password"].ToString();
        }

        public void Subscribe()
        {
            // Set up the RabbitMQ connection and channel
            var connectionFactory = new ConnectionFactory
            {
                HostName = m_HostName,
                Port = m_Port,
                UserName = m_UserName,
                Password = m_Password,
                RequestedFrameMax = UInt32.MaxValue
            };

     
            using (m_Connection = connectionFactory.CreateConnection())
            {
                using (m_Channel = m_Connection.CreateModel())
                {

                    // This instructs the channel not to prefetch more than one message
                    m_Channel.BasicQos(0, 1, false);

                    // DEAD LETTER QUEUE
                    IDictionary<string, object> arguments = new Dictionary<string, object>()
                        {
                           {"x-dead-letter-exchange", m_ExchangeName_DeadLetter}
                                };
                    m_Channel.ExchangeDeclare(m_ExchangeName_DeadLetter, ExchangeType.Fanout, true, false, arguments);
                    m_Channel.QueueDeclare(m_QueueName_DeadLetter, true, false, false, arguments);
                    m_Channel.QueueBind(m_QueueName_DeadLetter, m_ExchangeName_DeadLetter, m_RoutingKey_DeadLetter, arguments);

                    // NOTIFICATION QUEUE
                    m_Channel.ExchangeDeclare(m_ExchangeName, ExchangeType.Fanout, true, false, arguments);
                    m_Channel.QueueDeclare(m_QueueName, true, false, false, arguments);
                    m_Channel.QueueBind(m_QueueName, m_ExchangeName, m_RoutingKey, arguments);


                    m_Consumer = new EventingBasicConsumer(m_Channel);
                    m_Consumer.Received += (s, ev) =>
                    {
                        //Handle messages here.
                        try
                        {
                            var body = ev.Body.ToArray();
                            string message = Encoding.UTF8.GetString(body);
                            var obj = JsonConvert.DeserializeObject<Event>(message);
                            using (var dbCon = new LoggingContext())
                            {
                                dbCon.Events.Add(obj);
                                dbCon.SaveChanges();
                            }
                        }
                        catch(Exception ex)
                        {
                            LogUtility.WriteInfo(ex.Message);
                        }
                    };

                    m_Channel.BasicConsume(queue: m_QueueName, autoAck: true, consumer: m_Consumer);

                }
            }
        }

        public void Dispose()
        {
            if (m_Channel.IsOpen)
            {
                m_Channel.Close();
                m_Channel.Dispose();
            }
            if (m_Connection.IsOpen)
            {
                m_Connection.Close();
                m_Connection.Dispose();
            }
        }
    }
}
