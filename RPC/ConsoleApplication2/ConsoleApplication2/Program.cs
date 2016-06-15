using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
using System.Security.Policy;
using System.Runtime.Remoting.Activation;

namespace ConsoleApplication2
{
    class Program
    {
        static void Main(string[] args)
        {
            ChannelServices.RegisterChannel(new TcpClientChannel("h1", new BinaryClientFormatterSinkProvider()));
            ClassLibrary1.Class1 cla = (ClassLibrary1.Class1)Activator.GetObject(typeof(ClassLibrary1.Class1), "tcp://114.212.85.35:6767/CurrentTime", null);
            //RemotingConfiguration.RegisterWellKnownClientType(typeof(ClassLibrary1.Class1), "tcp://114.212.85.35:6767/CurrentTime");
            //ClassLibrary1.Class1 cla = new ClassLibrary1.Class1();
            DateTime dt = cla.getCurrentTime();
            Console.WriteLine(dt.ToLongTimeString());
            Console.ReadLine();
            //IChannel chan = new TcpClientChannel("channel1", new BinaryClientFormatterSinkProvider());
            //ChannelServices.RegisterChannel(chan, false);
            //WellKnownClientTypeEntry entry = new WellKnownClientTypeEntry(typeof(ClassLibrary1.Class1), "Time");
            //RemotingConfiguration.RegisterWellKnownClientType(entry);
            Console.ReadLine();
        }
    }
}
