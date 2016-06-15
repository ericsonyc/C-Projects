using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ObserverClient
{
    public class Observer:MarshalByRefObject
    {
        //public DateTime dateTime = DateTime.MinValue;
        public Observer()
        {

        }
        public virtual void Update()
        {
            //Console.WriteLine("hello");
        }
    }
}
