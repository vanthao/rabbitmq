using Ncua.Logging.DAL;
using Ncua.Logging.Entity;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace Ncua.CUOnline.EventServiceHost
{
    partial class AuditLogingService : ServiceBase
    {
        protected Thread m_WorkerThread;
        protected bool m_ServiceStarted = true;

        

        public AuditLogingService()
        {
            InitializeComponent();
            
        }

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            var logManager = new AuditLogManager();
            ThreadStart st = new ThreadStart(logManager.Subscribe);
            m_WorkerThread = new Thread(st);

            // set flag to indicate worker thread is active
            m_ServiceStarted = true;
            // start the thread
            m_WorkerThread.Start();
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }

        
    }
}
