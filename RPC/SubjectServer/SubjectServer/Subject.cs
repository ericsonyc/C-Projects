using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

namespace SubjectServer
{
    public class Subject : MarshalByRefObject
    {
        Socket serverSocket;
        List<TimerCurrent.Class1> observers = new List<TimerCurrent.Class1>();
        byte[] result = new byte[1024];
        static TimerCurrent.Class1 obs = null;
        int myPort = 8980;
        public Subject()
        {
            
        }
        public void Attach()
        {
            TcpChannel chan = new TcpChannel(7676);
            ChannelServices.RegisterChannel(chan, false);
            WellKnownServiceTypeEntry entry = new WellKnownServiceTypeEntry(typeof(TimerServer.Timer1), "CurrentTime", WellKnownObjectMode.SingleCall);
            RemotingConfiguration.RegisterWellKnownServiceType(entry);
            IPAddress ip = new IPAddress(Dns.GetHostByName(Dns.GetHostName()).AddressList[0].Address);
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip, myPort));
            serverSocket.Listen(100);
            Thread myThread = new Thread(ListenClientConnect);
            myThread.Start();
        }

        private void ListenClientConnect()
        {
            IPAddress ip = new IPAddress(Dns.GetHostByName(Dns.GetHostName()).AddressList[0].Address);
            while (true)
            {
                Socket clientSocket = serverSocket.Accept();
                clientSocket.Send(Encoding.ASCII.GetBytes(ip.ToString()+":7676"));
                Thread receiveThread = new Thread(ReceiveMessage);
                receiveThread.Start(clientSocket);
            }
        }

        private void ReceiveMessage(object clientSocket)
        {
            Socket myClientSocket = (Socket)clientSocket;
            
            while (true)
            {
                try
                {
                    int receiveNumber = myClientSocket.Receive(result);
                    //Console.WriteLine("接收客户端{0}消息{1}", myClientSocket.RemoteEndPoint.ToString(), Encoding.ASCII.GetString(result, 0, receiveNumber));
                    if (receiveNumber!=0)
                    {
                        IPEndPoint point = (IPEndPoint)myClientSocket.RemoteEndPoint;
                        string path = point.Address.ToString();
                        string port = Encoding.ASCII.GetString(result,0,receiveNumber);
                        string tcp = "tcp://" + path + ":" + port + "/Up";
                        obs = (TimerCurrent.Class1)Activator.GetObject(typeof(TimerCurrent.Class1), tcp, null);
                        //obs.Update();
                        if (obs is TimerCurrent.Class1 && obs != null)
                            observers.Add(obs);
                        
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    //Ditach(obs);
                    myClientSocket.Shutdown(SocketShutdown.Both);
                    myClientSocket.Close();
                    break;
                }
            }
        }

        public void Ditach(TimerCurrent.Class1 ob)
        {
            observers.Remove(ob);
        }

        public virtual  void Notify()
        {
            for (int i = 0; i < observers.Count; i++)
            {
                TimerCurrent.Class1 ob = observers[i];
                try
                {
                    ob.Update();
                }
                catch (Exception ex)
                {
                    //observers.RemoveAt(observers.IndexOf(ob));
                    Console.WriteLine(ex.Message);
                }
                
            }    
        }
    }
}
