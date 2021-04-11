using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ncua.Logging.Entity
{
    public class EventAttribute
    {
        public int EventAttributeId { get; set; }
        public int EventTypeId { get; set; }
        public string Desc { get; set; }
        public string CreateId { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateId { get; set; }
        public DateTime UpdateDate { get; set; }
        public EventType EventType { get; set; }
    }
}
