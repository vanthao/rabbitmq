using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ncua.Logging.Entity
{
    public class EventType
    {
        public int EventTypeId { get; set; }
        public string Desc { get; set; }
        public string CreateId { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateId { get; set; }
        public DateTime UpdateDate { get; set; }
        public List<Event> Events { get; set; }
        public List<EventAttribute> EventAttributes { get; set; }

        public EventType()
        {
            Events = new List<Event>();
            EventAttributes = new List<EventAttribute>();
        }
    }
}
