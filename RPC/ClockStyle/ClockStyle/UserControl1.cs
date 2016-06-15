using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace ClockStyle
{
    public partial class UserControl1 : UserControl
    {
        public DateTime now;
        private TimerCurrent.Class1 tim;
        private Graphics g;
        private Pen pen;
        private FontFamily MyFamily;
        private Font MyFont;
        private int w;
        private int h;
        private string ipadd;
        public TimerCurrent.Class1 Tim
        {
            set { tim = value; }
        }
        public string IPAdd
        {
            set { ipadd = value; }
        }
        public UserControl1()
        {
            InitializeComponent();
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias; //
            g.SmoothingMode = SmoothingMode.HighQuality;//绘图模式默认为粗糙模式，将会出现锯齿！

            w = pictureBox1.Width;
            h = pictureBox1.Height;
            int x1 = pictureBox1.Location.X;
            int y1 = pictureBox1.Location.Y;

            //g.FillEllipse(Brushes.Black, x1 + 2, y1 + 2, w - 4, h - 4); //外圆
            //MyFamily = new System.Drawing.FontFamily("Impact"); //字体
            //MyFont = new System.Drawing.Font(MyFamily, 20, FontStyle.Bold, GraphicsUnit.Pixel);
            //g.DrawString("HOCYLAN", MyFont, Brushes.Yellow, x1 + w - 180, y1 + h - 100);
            MyFamily = new System.Drawing.FontFamily("Times New Roman");
            MyFont = new System.Drawing.Font(MyFamily, 18, FontStyle.Bold, GraphicsUnit.Pixel);
            pen = new Pen(Color.Black, 2);
            //g.DrawEllipse(pen, x1 + 7, y1 + 7, w - 13, h - 13);// 内圆
            g.TranslateTransform(x1 + (w / 2), y1 + (h / 2));//重新设置坐标原点
            g.FillEllipse(Brushes.Black, -5, -5, 10, 10);//绘制表盘中心
            //g.TranslateTransform(w/2,h/2);
            for (int x = 0; x < 60; x++) //小刻度
            {
                g.FillRectangle(Brushes.Black, new Rectangle(-2, (System.Convert.ToInt16(h - 8) / 2 - 2) * (-1), 3, 10));
                g.RotateTransform(6);//偏移角度
            }
            for (int i = 12; i > 0; i--) //大刻度
            {
                string myString = i.ToString();
                //绘制整点刻度
                g.FillRectangle(Brushes.Black, new Rectangle(-3, (System.Convert.ToInt16(h - 8) / 2 - 2) * (-1), 6, 20));
                //绘制数值
                g.DrawString(myString, MyFont, Brushes.Black, new PointF(myString.Length * (-6), (h - 8) / -2 + 26));
                //顺时针旋转度
                g.RotateTransform(-30);//偏移角度
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        public void pictureInvalidate()
        {
            this.pictureBox1.Invalidate();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            this.pictureBox1.BackColor = Color.Transparent;
            g = e.Graphics;
            //绘图模式默认为粗糙模式，将会出现锯齿！
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TranslateTransform(w / 2, h / 2);//重新设置坐标原点
            //获得系统时间值
            now = tim.Datatime;
            int second = now.Second;
            int minute = now.Minute;
            int hour = now.Hour;

            //绘秒针
            pen = new Pen(Color.Red, 1);
            pen.EndCap = LineCap.ArrowAnchor;
            g.RotateTransform(6 * second);
            float y = (float)((-1) * (h / 2.75));
            g.DrawLine(pen, new PointF(0, 0), new PointF((float)0, y));
            ////绘分针
            pen = new Pen(Color.Blue, 4);
            // pen.EndCap = LineCap.ArrowAnchor;
            g.RotateTransform(-6 * second); //恢复系统偏移量，再计算下次偏移
            g.RotateTransform((float)(second * 0.1 + minute * 6));
            y = (float)((-1) * ((h - 20) / 2.75));
            g.DrawLine(pen, new PointF(0, 0), new PointF((float)0, y));
            ////绘时针
            pen = new Pen(Color.Yellow, 6);
            // pen.EndCap = LineCap.ArrowAnchor;
            g.RotateTransform((float)(-second * 0.1 - minute * 6));//恢复系统偏移量，再计算下次偏移
            g.RotateTransform((float)(second * 0.01 + minute * 0.1 + hour * 30));
            y = (float)((-1) * ((h - 35) / 2.75));
            g.DrawLine(pen, new PointF(0, 0), new PointF((float)0, y));
        }
    }
}
