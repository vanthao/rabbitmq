using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ncua.Logging.Entity
{
    public class EventDetail
    {
        public long EventDetailId { get; set; }
        public long EventId { get; set; }
        public int EventAttributeId { get; set; }
        public string Value { get; set; }
        public string CreateId { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateId { get; set; }
        public DateTime UpdateDate { get; set; }
        public EventAttribute EventAttribute { get; set; }
        public Event Event { get; set; }
    }
}
