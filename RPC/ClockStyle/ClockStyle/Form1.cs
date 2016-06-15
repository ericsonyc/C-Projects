using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ClockStyle
{
    public partial class Form1 : Form
    {
        private TimerCurrent.Class1 tim;
        private UserControl1 user;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tim = new TimerCurrent.Class1();
            user = new UserControl1();
            this.userControl11.Tim = tim;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tim.IpAdd = this.ipText1.Text;
            tim.QiDong();
            timer1.Interval = 50;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.userControl11.pictureInvalidate();
        }
    }
}
