using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV.Util;
using System.Collections;
using Emgu.CV.Structure;
using Emgu.CV;

namespace Vision
{
    public partial class Form1 : Form
    {
        ArrayList hash=null;
        ArrayList filePaths = null;
        Hashtable pictures = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Title = "选择图片";
            openFile.Filter = "图片文件|*.bmp;*.jpg;*.jpeg;*.gif;*.png";
            openFile.Multiselect = true;
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                int length = imageList1.Images.Count;
                for (int i = length; i > 0;i-- )
                {
                    imageList1.Images.RemoveAt(i-1);
                    listView1.Items.RemoveAt(i-1);
                }
                pictures = new Hashtable();
                string[] files = openFile.FileNames;
                hash = new ArrayList();
                filePaths = new ArrayList();
                imageList1.ImageSize = new Size(200, 200);
                for (int i = 0; i < files.Length; i++)
                {
                    Image bmp = Bitmap.FromFile(files[i]);
                    imageList1.Images.Add(bmp);
                    filePaths.Add(files[i]);
                    hash.Add(null);
                }
                listView1.LargeImageList = imageList1;
                for (int i = 0; i < imageList1.Images.Count;i++ )
                {
                    ListViewItem item = new ListViewItem("", i);
                    item.ImageIndex = i;
                    listView1.Items.Add(item);
                }
            }            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择图像", "错误框", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int count = listView1.SelectedItems.Count;
                for (int i = 0; i < count; i++)
                {
                    int index = listView1.SelectedItems[i].ImageIndex;
                    Image image = imageList1.Images[index];
                    Image img=this.returnImage(image);
                    pictures.Add("picture"+i, new Picture("picture"+i));
                    MCvScalar scalar, scalar1;
                    Image<Bgr, Byte> ecanny = new Image<Bgr, Byte>(new Bitmap(img));
                    Image<Bgr, Byte> ecanny1 = new Image<Bgr, byte>(new Bitmap(image));
                    for (int j = 0; j < ecanny.Height; j++)
                    {
                        if (j == 0 || j == ecanny.Height - 1)
                        {
                            for (int k = 0; k < ecanny.Width; k++)
                            {
                                scalar = CvInvoke.cvGet2D(ecanny.Ptr, j, k);
                                if (scalar.v0 > 0 && scalar.v1 > 0 && scalar.v2 > 0)
                                {
                                    scalar.v0 = 0;
                                    scalar.v1 = 0;
                                    scalar.v2 = 255;
                                    CvInvoke.cvSet2D(ecanny1.Ptr, j, k, scalar);
                                    if (j == 0)
                                    {
                                        this.diedaiPicture(k, j, ref ecanny, ref ecanny1, 0, 1,i);
                                    }
                                    else
                                    {
                                        this.diedaiPicture(k, j, ref ecanny, ref ecanny1, 2, 1,i);
                                    }
                                }
                            }
                        }
                        else
                        {
                            scalar = CvInvoke.cvGet2D(ecanny.Ptr, j, 0);
                            if (scalar.v0 > 0 && scalar.v1 > 0 && scalar.v2 > 0)
                            {
                                scalar.v0 = 0;
                                scalar.v1 = 0;
                                scalar.v2 = 255;
                                CvInvoke.cvSet2D(ecanny1.Ptr, j, 0, scalar);
                                this.diedaiPicture(0, j, ref ecanny, ref ecanny1, 1, 1,i);
                            }
                            scalar = CvInvoke.cvGet2D(ecanny.Ptr, j, ecanny.Width-1);
                            if (scalar.v0 > 0 && scalar.v1 > 0 && scalar.v2 > 0)
                            {
                                scalar.v0 = 0;
                                scalar.v1 = 0;
                                scalar.v2 = 255;
                                CvInvoke.cvSet2D(ecanny1.Ptr, j, ecanny.Width-1, scalar);
                                this.diedaiPicture(ecanny.Width-1, j, ref ecanny, ref ecanny1, 3, 1,i);
                            }
                        }
                    }
                    imageList1.Images[index] = ecanny1.Bitmap;
                    listView1.RedrawItems(0, listView1.Items.Count - 1, false);
                    /*MCvScalar scalar;
                    MCvScalar scalar1;
                    Image<Bgr,Byte> ecanny=new Image<Bgr,Byte>(new Bitmap(img));
                    Image<Bgr,Byte> ecan=new Image<Bgr,Byte>(new Bitmap(image));
                    for (int j = 0; j < ecanny.Height; j++)
                    {
                        for (int k = 0; k < ecanny.Width; k++)
                        {
                            scalar = CvInvoke.cvGet2D(ecanny.Ptr, j, k);
                            if (scalar.v0>0&&scalar.v1>0&&scalar.v2>0)
                            {
                                scalar.v0 = 0;
                                scalar.v1 = 0;
                                scalar.v2 = 255;
                                scalar1 = CvInvoke.cvGet2D(ecan.Ptr, j, k);
                                CvInvoke.cvSet2D(ecanny.Ptr, j, k, scalar);
                            }
                        }
                    }
                    //CvInvoke.cvNamedWindow("tuxiang");
                    //CvInvoke.cvShowImage("dfga", ecanny.Ptr);
                    hash[index] = ecanny.Bitmap;
                    imageList1.Images[index] = ecanny.Bitmap;
                    listView1.RedrawItems(0, listView1.Items.Count - 1, false);*/
                }
            }
        }

        private void diedaiPicture(int xIndex, int yIndex, ref Image<Bgr, Byte> ecanny, ref Image<Bgr, Byte> ecanny1, int way,int times,int index)
        {
            if(times>=20)
                return;
            Picture picture=(Picture)pictures[("picture"+index)];
            if (way == 0)
            {
                MCvScalar scalar;
                if (times == 1)
                {
                    picture.way0.Add(new int[] { yIndex, xIndex });
                }
                for (int k = yIndex; k < yIndex+2; k++)
                {
                    int i = xIndex - 1 > 0 ? xIndex - 1 : 0;
                    int j = xIndex + 1 >= ecanny.Width ? ecanny.Width : xIndex + 2;
                    for (; i < j; i++)
                    {
                        scalar = CvInvoke.cvGet2D(ecanny.Ptr, k, i);
                        if (scalar.v0 > 0 && scalar.v1 > 0 && scalar.v2 > 0)
                        {
                            scalar.v0 = 0;
                            scalar.v1 = 0;
                            scalar.v2 = 255;
                            CvInvoke.cvSet2D(ecanny1.Ptr, k, i, scalar);
                            
                            if (k != yIndex || i != xIndex)
                            {
                                this.diedaiPicture(i, k, ref ecanny, ref ecanny1, way, times + 1,index);
                            }
                            
                        }
                    }
                }
            }
            else if (way == 1)
            {
                MCvScalar scalar;
                if (times == 1)
                {
                    picture.way1.Add(new int[] { yIndex, xIndex });
                }
                int i = yIndex - 1 > 0 ? yIndex - 1 : 0;
                int j = yIndex + 1 >= ecanny.Height ? ecanny.Height : yIndex + 2;
                for (; i < j; i++)
                {
                    for (int k = xIndex; k < xIndex+2; k++)
                    {
                        scalar = CvInvoke.cvGet2D(ecanny.Ptr, i, k);
                        if (scalar.v0 > 0 && scalar.v1 > 0 && scalar.v2 > 0)
                        {
                            scalar.v0 = 0;
                            scalar.v1 = 0;
                            scalar.v2 = 255;
                            CvInvoke.cvSet2D(ecanny1.Ptr, i, k, scalar);
                            
                            if (i != yIndex || k != xIndex)
                            {
                                this.diedaiPicture(k, i, ref ecanny, ref ecanny1, way, times + 1,index);
                            }
                            
                        }
                    }
                }
            }
            else if (way == 2)
            {
                MCvScalar scalar;
                if (times == 1)
                {
                    picture.way2.Add(new int[] { yIndex, xIndex });
                }
                int k = yIndex - 1 > 0 ? yIndex - 1 : 0;
                for (; k < yIndex+1; k++)
                {
                    int i = xIndex - 1 > 0 ? xIndex - 1 : 0;
                    int j = xIndex + 1 >= ecanny.Width ? ecanny.Width : xIndex + 2;
                    for (; i < j; i++)
                    {
                        scalar = CvInvoke.cvGet2D(ecanny.Ptr, k, i);
                        if (scalar.v0 > 0 && scalar.v1 > 0 && scalar.v2 > 0)
                        {
                            scalar.v0 = 0;
                            scalar.v1 = 0;
                            scalar.v2 = 255;
                            CvInvoke.cvSet2D(ecanny1.Ptr, k, i, scalar);
                            
                            if (k != yIndex || i != xIndex)
                            {
                                this.diedaiPicture(i, k, ref ecanny, ref ecanny1, way, times + 1,index);
                            }
                            
                        }
                    }
                }
            }
            else if (way == 3)
            {
                MCvScalar scalar;
                if (times == 1)
                {
                    picture.way3.Add(new int[] { yIndex, xIndex });
                }
                int i = yIndex - 1 > 0 ? yIndex - 1 : 0;
                int j = yIndex + 1 >= ecanny.Height ? ecanny.Height : yIndex + 2;
                for (; i < j; i++)
                {
                    int k = xIndex - 1 > 0 ? xIndex - 1 : 0;
                    for (; k < xIndex+1; k++)
                    {
                        scalar = CvInvoke.cvGet2D(ecanny.Ptr, i, k);
                        if (scalar.v0 > 0 && scalar.v1 > 0 && scalar.v2 > 0)
                        {
                            scalar.v0 = 0;
                            scalar.v1 = 0;
                            scalar.v2 = 255;
                            CvInvoke.cvSet2D(ecanny1.Ptr, i, k, scalar);
                            
                            if (i != yIndex || k != xIndex)
                            {
                                
                                this.diedaiPicture(k, i, ref ecanny, ref ecanny1, way, times + 1,index);
                            }
                            
                        }
                    }
                }
            }
        }

        private Image returnImage(Image image)
        {
            Image<Bgr, Byte> frame = new Image<Bgr, Byte>(new Bitmap(image));

            Image<Gray, Byte> Ecanny = frame.Convert<Gray, Byte>();

            CvInvoke.cvCanny(Ecanny.Ptr, Ecanny.Ptr, 50, 150, 3);
            //CvInvoke.cvSobel(Ecanny.Ptr, Ecanny.Ptr, 0, 1, 3);
            //cvCanny是opencv中常用的函数，原本的参数应该是IplImage*类型，这里使用Intpr代替，即Ecanny.ptr
            return Ecanny.Bitmap;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择要合并的图像", "错误框", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MIplImage mipl = new MIplImage();
                Image<Bgr, Byte> img = new Image<Bgr, Byte>(405, 200);
                IntPtr ptr = CvInvoke.cvCreateImage(new Size(405, 200), Emgu.CV.CvEnum.IPL_DEPTH.IPL_DEPTH_8U, 3);
                img.Ptr = ptr;
                List<Image> images = new List<Image>();
                for (int i = 0; i < imageList1.Images.Count; i++)
                {
                    images.Add(imageList1.Images[i]);
                }
                Image<Bgr, Byte> img0 = new Image<Bgr, Byte>(new Bitmap(images[0]));
                MCvScalar scalar = new MCvScalar();
                for (int i = 0; i < img0.Height; i++)
                {
                    for (int j = 0; j < img0.Width; j++)
                    {
                        scalar = CvInvoke.cvGet2D(img0.Ptr, i, j);
                        CvInvoke.cvSet2D(img.Ptr, i, j, scalar);
                    }
                }
                for (int i = 0; i < img0.Height; i++)
                {
                    for (int j = img0.Width; j < img0.Width + 6; j++)
                    {
                        scalar.v0 = 255;
                        scalar.v1 = 255;
                        scalar.v2 = 255;
                        CvInvoke.cvSet2D(img.Ptr, i, j, scalar);
                    }
                }
                if (images.Count > 1)
                {
                    Image<Bgr, Byte> img1 = new Image<Bgr, Byte>(new Bitmap(images[1]));
                    for (int i = 0; i < img1.Height; i++)
                    {
                        for (int j = img0.Width + 5; j < img.Width; j++)
                        {
                            scalar = CvInvoke.cvGet2D(img1.Ptr, i, j - img0.Width - 5);
                            CvInvoke.cvSet2D(img.Ptr, i, j, scalar);
                        }
                    }
                }
                CvInvoke.cvShowImage("合成", img.Ptr);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("请选择图像", "错误框", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                int count = listView1.SelectedItems.Count;
                for (int i = 0; i < count; i++)
                {
                    int index = listView1.SelectedItems[i].ImageIndex;
                    Image image = imageList1.Images[index];
                    Image img = this.returnImage(image);
                    imageList1.Images[index] = this.returnImage(img);
                    listView1.RedrawItems(0, listView1.Items.Count - 1, false);
                }
            }
        }
    }
}
