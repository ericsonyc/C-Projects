using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Client
{
    public partial class Form1 : Form
    {
        TimerCurrent.Class1 cla;
        public delegate void setValueEventHandle();
        setValueEventHandle hello;
        Thread th;
        IPText iptext;
        public Form1()
        {
            InitializeComponent();
            cla = new TimerCurrent.Class1();
            //hello = new setValueEventHandle(setValue);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        public void setValue()
        {
            while (true)
            {
                this.label2.Text = cla.Datatime.ToLongTimeString();
                Thread.Sleep(50);
            }
        }

        public void getTime()
        {
            this.Invoke(hello);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.label2.Text = cla.Datatime.ToLongTimeString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cla.IpAdd = this.ipText1.Text;
            cla.QiDong();
            timer1.Interval = 50;
            timer1.Start();
        }
    }
}
