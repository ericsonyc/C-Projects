using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;

namespace Observer
{
    public class Observer
    {
        public virtual void Update()
        {
            TcpChannel chan = new TcpChannel(8989);
            ChannelServices.RegisterChannel(chan, false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(Observer), "tcp://localhost:8989/Update", WellKnownObjectMode.SingleCall);
        }
    }
}
