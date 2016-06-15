using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting;

namespace TimerServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.label1.Text = DateTime.Now.ToLongTimeString();
            Subject.Subject sub = new Subject.Subject();
            sub.Notify();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TcpChannel chan = new TcpChannel(5656);
            ChannelServices.RegisterChannel(chan, false);
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(TimerSubject.Timer1), "tcp://localhost:5656/getCurrentTime", WellKnownObjectMode.SingleCall);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
