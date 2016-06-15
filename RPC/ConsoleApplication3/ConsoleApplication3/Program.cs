using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace ConsoleApplication3
{
    class Program
    {
        static void Main(string[] args)
        {
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            TcpListener listener = new TcpListener(ip, 8787);
            listener.Start();
            while (true)
            {
                Socket socket = listener.AcceptSocket();
                NetworkStream stream = new NetworkStream(socket);
                StreamReader sr = new StreamReader(stream);
                Console.WriteLine(sr.ReadLine());
            }
        }
    }
}
