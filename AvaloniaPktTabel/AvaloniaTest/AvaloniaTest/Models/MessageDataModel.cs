using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaTest.Models
{
    public class MessageDataModel
    {

        public int MsgIndex { get; set; }
        
        public string MsgType { get; set; }

        public double MsgStartTime { get; set; }

        public double MsgStopTime { get; set; }

        public string MsgPowerRole { get; set; }
    }
}
