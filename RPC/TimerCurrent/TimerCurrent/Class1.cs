using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;
using System.Runtime.Serialization;
using System.Runtime.Remoting.Channels.Http;

namespace TimerCurrent
{
    public class Class1 : MarshalByRefObject
    {
        public static DateTime dateTime = new DateTime();
        static TimerServer.Timer1 tim;
        private string ipaddress;
        public string IpAdd
        {
            set
            {
                ipaddress = value;
            }
        }
        public DateTime Datatime
        {
            get { return dateTime; }
            set { dateTime = value; }
        }
        public Class1()
        {
            //QiDong();
        }

        public void QiDong()
        {
            IPAddress ip = IPAddress.Parse("114.212.85.35");
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                clientSocket.Connect(new IPEndPoint(ip, 8980));
                byte[] result = new byte[1024];
                int recevieNumber = clientSocket.Receive(result);
                if (recevieNumber != 0)
                {
                    string tcp = "tcp://" + Encoding.ASCII.GetString(result, 0, recevieNumber)+"/CurrentTime";
                    tim = (TimerServer.Timer1)Activator.GetObject(typeof(TimerServer.Timer1), tcp, null);
                    dateTime = tim.getCurrentTime();
                    clientSocket.Send(Encoding.ASCII.GetBytes("7878"));
                    TcpServerChannel chan = new TcpServerChannel("Update", 7878);
                    Console.WriteLine(chan.ChannelName);
                    ChannelServices.RegisterChannel(chan, false);
                    WellKnownServiceTypeEntry entry = new WellKnownServiceTypeEntry(typeof(TimerCurrent.Class1), "Up", WellKnownObjectMode.SingleCall);
                    RemotingConfiguration.RegisterWellKnownServiceType(entry);
                }
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine("连接服务器失败");
                //clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }

        }

        public void Update()
        {
            //tim = (TimerServer.Timer1)Activator.GetObject(typeof(TimerServer.Timer1), "tcp://114.212.85.35:7676/getCurrentTime", null);
            DisplayTime();
        }

        public void DisplayTime()
        {
            dateTime = tim.getCurrentTime();
            
        }

    }
}
