using System;
using System.Collections.Generic;
using System.Text;

namespace Ncua.AuditLog.EventTypes
{
    public class CustomMessage
    {
       public object Message { get; set; }
       public string[] MessageType { get; set; }
    }
}
