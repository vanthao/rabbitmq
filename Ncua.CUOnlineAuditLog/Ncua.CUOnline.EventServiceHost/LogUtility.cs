using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Ncua.CUOnline.EventServiceHost
{
    public class LogUtility
    {
        public static readonly string CuOnlineWebServicesEventLogSourceName = "CUOnline Private Web Services";

        public LogUtility()
        {

        }

       
        public static void WriteError(string source, Exception ex)
        {
            var message = String.Format("{0}{1}{2}", source, Environment.NewLine, ex);

            WriteError(message);
        }

        public static void WriteError(string message)
        {
            Console.WriteLine(String.Format("ERROR -- {0} -- {1}", CuOnlineWebServicesEventLogSourceName, message));

            EventLog.WriteEntry(CuOnlineWebServicesEventLogSourceName, message, EventLogEntryType.Error);
        }

        public static void WriteInfo(string message)
        {
            Console.WriteLine(String.Format("INFO -- {0} -- {1}", CuOnlineWebServicesEventLogSourceName, message));

            EventLog.WriteEntry(CuOnlineWebServicesEventLogSourceName, message, EventLogEntryType.Information);
        }

        public static void WriteWarning(string message)
        {
            Console.WriteLine(String.Format("WARNING -- {0} -- {1}", CuOnlineWebServicesEventLogSourceName, message));

            EventLog.WriteEntry(CuOnlineWebServicesEventLogSourceName, message, EventLogEntryType.Warning);
        }

        public static void WriteDebug(string message)
        {
            Console.WriteLine(String.Format("DEBUG -- {0} -- {1}", CuOnlineWebServicesEventLogSourceName, message));

            EventLog.WriteEntry(CuOnlineWebServicesEventLogSourceName, message, EventLogEntryType.Information);
        }
    }
}
