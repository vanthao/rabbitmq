using System;
using System.ServiceProcess;

namespace Ncua.CUOnline.EventServiceHost
{
    class Program
    {
        static void Main()
        {
            var servicesToRun = new ServiceBase[]
                                    {
                                        new AuditLogingService()
                                    };


            ServiceBase.Run(servicesToRun);
        }

    }
}
