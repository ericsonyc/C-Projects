using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace ClockControl
{
    public partial class PointClock : UserControl
    {
        public DateTime date;
        public PointClock()
        {
            InitializeComponent();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            this.SetStyle(ControlStyles.ResizeRedraw, true);

            this.timer1.Start();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // init the origin
            g.TranslateTransform(this.Width / 2.0f, this.Height / 2.0f);

            int dialRadius = Math.Min(this.Width, this.Height) / 2;

            // Draw the clock dial
            GraphicsState state = g.Save();

            for (int i = 0; i < 60; i++)
            {
                int radius = 15;
                if (i % 5 == 0)
                    radius = 25;

                g.FillEllipse(Brushes.Blue, new Rectangle(-radius / 2, -dialRadius, radius, radius));

                g.RotateTransform(360 / 60);
            }

            g.Restore(state);

            // Get current time
            DateTime now = DateTime.Now;

            // Draw hour hand
            state = g.Save();

            g.RotateTransform((Math.Abs(now.Hour - 12) + now.Minute / 60f) * 360f / 12f);
            g.FillRectangle(Brushes.Black, new Rectangle(-5, -dialRadius + 50, 10, dialRadius - 40));

            g.Restore(state);

            // Draw Minute hand
            state = g.Save();

            g.RotateTransform((now.Minute + now.Second / 60f) * 360f / 60f);
            g.FillRectangle(Brushes.DarkGreen, new Rectangle(-3, -dialRadius + 30, 6, dialRadius - 15));

            g.Restore(state);

            // Draw Second hand
            state = g.Save();

            g.RotateTransform(now.Second * 360f / 60f);
            g.FillRectangle(Brushes.Red, new Rectangle(-1, -dialRadius + 10, 2, dialRadius));

            g.Restore(state);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Invalidate();
        }
    }
}
