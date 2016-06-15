using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Http;

namespace TimerServer
{
    public class Timer1 : SubjectServer.Subject
    {
        public Timer1():base()
        {
            
        }
        public DateTime getCurrentTime()
        {
            return DateTime.Now;
        }

    }
}
