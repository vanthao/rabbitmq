using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ncua.Logging.Entity
{
    public class Event
    {
        public long EventId { get; set; }
        public int EventTypeId { get; set; }
        public int CorrelationId { get; set; }
        public string AppDomainName { get; set; }
        public string MachineName { get; set; }
        public string Message { get; set; }
        public int Priority { get; set; }
        public int ProcessId { get; set; }
        public string ProcessName { get; set; }
        public string Severity { get; set; }
        public string ThreadName { get; set; }
        public DateTime GenerateDate { get; set; }
        public string CreateId { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateId { get; set; }
        public DateTime UpdateDate { get; set; }
        public List<EventAttribute> EventAttributes { get; set; }
        public List<EventDetail> EventDetails { get; set; }
        public EventType EventType { get; set; }
        public Event()
        {
            EventAttributes = new List<EventAttribute>();
        }
    }
}
