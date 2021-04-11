using MassTransit;
using Ncua.AuditLog.Attributes;
using Ncua.AuditLog.EventTypes;
using Ncua.CUOnline.EventServiceHost;
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
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        protected Thread m_WorkerThread;
        protected bool m_ServiceStarted = true;

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
        private EventingBasicConsumer m_Consumer;

        private void LogQueue()
        {

            var audit = new AuditEvent();

            audit.Severity = System.Diagnostics.TraceEventType.Information;
            audit.EventAttributes[AuditAttribute.AuditType] = "Sample Audit";
            audit.EventAttributes[AuditAttribute.CreditUnion] = "Sample CreditUnion";

            audit.Log();
        }
        private void GetConfig()
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

        public void GetQueue()
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

            var retryCount = 0;
            var connectionSuccess = false;

            while (retryCount < 3 && !connectionSuccess)
            {
                try
                {
                    m_Connection = connectionFactory.CreateConnection();
                    connectionSuccess = true;
                }
                catch (Exception ex)
                {
                    retryCount++;
                    connectionSuccess = false;
                    Thread.Sleep(20000);
                }
                m_Connection = null;
            }

            using (m_Connection = connectionFactory.CreateConnection())
            {
                using (var m_Channel = m_Connection.CreateModel())
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
                    m_Channel.ExchangeDeclare(m_ExchangeName, ExchangeType.Fanout, true, false, null);
                    m_Channel.QueueDeclare(m_QueueName, true, false, false, null);
                    m_Channel.QueueBind(m_QueueName, m_ExchangeName, m_RoutingKey, null);


                    m_Consumer = new EventingBasicConsumer(m_Channel);
                    m_Consumer.Received += (s, ev) =>
                    {
                        //Handle messages here.
                        var body = ev.Body.ToArray();
                        string message = Encoding.UTF8.GetString(body);
                        var obj = JsonConvert.DeserializeObject<Event>(message);
                        using (var dbCon = new LoggingContext())
                        {
                            dbCon.Events.Add(obj);
                            dbCon.SaveChanges();
                        }
                    };

                    m_Channel.BasicConsume(queue: m_QueueName, autoAck: true, consumer: m_Consumer);
                }
            }

        }

        public static async Task Main()
        {

            var Pro = new Program();
            //for (var i = 0; i < 20; i++)
            //{
            //    Pro.LogQueue();
            //}

            //var svc = new AuditLogManager();
            //svc.Subscribe();
            //if (Pro.m_Channel.IsOpen)
            //{
            //    Pro.m_Channel.Close();
            //    Pro.m_Channel.Dispose();
            //}
            //if (Pro.m_Connection.IsOpen)
            //{
            //    Pro.m_Connection.Close();
            //    Pro.m_Connection.Dispose();
            //}
            await Dequeue();
        }

        public static async Task Dequeue()
        {
            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.Host("localhost", "/", h => {
                    h.Username("cuonline");
                    h.Password("@1775Duke");
                });
                
                cfg.ReceiveEndpoint("CuOnline_SystemLog_Queue", ep =>
                {
                    ep.Consumer<OrderSubmittedEventConsumer>();
                   
                    ep.PrefetchCount = 1;
                   
                });

            });

            var source = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await busControl.StartAsync(source.Token);
            try
            {
                Console.WriteLine("Press enter to exit");

                await Task.Run(() => Console.ReadLine());
            }
            finally
            {
                await busControl.StopAsync();
            }
        }
    }
}
