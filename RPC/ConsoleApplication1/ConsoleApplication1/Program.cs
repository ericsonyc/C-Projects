using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpChannel chan = new TcpChannel(6767);
            ChannelServices.RegisterChannel(chan, false);
            WellKnownServiceTypeEntry entry = new WellKnownServiceTypeEntry(typeof(ClassLibrary1.Class1), "CurrentTime", WellKnownObjectMode.Singleton);
            RemotingConfiguration.RegisterWellKnownServiceType(entry);
            //RemotingConfiguration.ApplicationName = "stru";
            //RemotingConfiguration.RegisterActivatedServiceType(typeof(ClassLibrary1.Class1));
            Console.ReadLine();
            
            //Console.WriteLine(cla.getCurrentTime().ToLongTimeString());
            Console.ReadLine();
        }
    }
}
