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

namespace Server
{
    public partial class Form1 : Form
    {
        SubjectServer.Subject sub;
        public Form1()
        {
            InitializeComponent();
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToLongTimeString();
            sub.Notify();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Text = "服务器";
        }
            
        private void button1_Click(object sender, EventArgs e)
        {
            if (sub == null)
            {
                sub = new TimerServer.Timer1();
                sub.Attach();
            }
                
            this.timer1.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

    }
}
