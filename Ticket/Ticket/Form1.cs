using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using OpenCvSharp;

namespace Ticket
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(String class_Name, String Window_Name);

        [DllImport("user32.dll")]
        internal static extern bool GetWindowPlacement(IntPtr handle, ref WINDOWPLACEMENT placement);

        [DllImport("user32.dll")]
        internal static extern bool PrintWindow(IntPtr hWnd, IntPtr hdcBit, int nFlags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dvFlags, uint dx, uint dy, uint cButtons, uint dvExtraInfo);

        private const int MOUSEEVENTF_LEFTDOWN = 0x02;

        private const int MOUSEEVENTF_LEFTUP = 0x04;

        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;

        private const int MOUSEEVENTF_RIGHTUP = 0x10;


        //좌석색상
        public Color Color_Purple = Color.FromArgb(255, 123, 104, 238);
        public Color Color_DrakGreen = Color.FromArgb(255, 28, 168, 20);
        public Color Color_SkyBlue = Color.FromArgb(255, 23, 179, 255);
        public Color Color_Orange = Color.FromArgb(255, 251, 126, 78);
        public Color Color_LightGreen = Color.FromArgb(255, 160, 213, 63);
        public Color Color_Melon = Color.FromArgb(255, 186, 168, 130);

        IntPtr FindHnd;
        Bitmap Confirm_Img = null;
        Bitmap Place1_Img = null;
        Bitmap Place2_Img = null;

        
        internal enum SHOW_WINDOW_COMMANDS : int
        {
            HIDE = 0,
            NORMAL = 1,
            MINIMIZED = 2,
            MAXIMIZED = 3,
        }

        internal struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public SHOW_WINDOW_COMMANDS showc_cmd;
            public System.Drawing.Point min_position;
            public System.Drawing.Point max_position;
            public Rectangle normal_position;
        }

        internal struct Ip_Pos
        {
            public int Dig_X;
            public int Dig_Y;
        }

        Ip_Pos pPos = new Ip_Pos();


        public Form1()
        {
            InitializeComponent();
            initial();
            //찾을 이미지
            Confirm_Img = new Bitmap(@"img\Confirm.PNG");
            Place1_Img = new Bitmap(@"img\Place1.PNG");
            Place2_Img = new Bitmap(@"img\Place2.PNG");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String tmp = "";

            Bitmap ScreenBmp;

            //장수
            int nCurNum; 

            //상단 바 이름
            tmp = "인터파크 티켓 - Chrome";
            FindHnd = FindWindow(null, tmp);
            nCurNum = comboBox1.SelectedIndex + 1;
            if (If_FindHandle(FindHnd) && nCurNum > 0 && comboBox2.SelectedIndex >= 0)
            {
                If_GetHandlePos(FindHnd);
                Cursor.Position = new System.Drawing.Point(pPos.Dig_X, pPos.Dig_Y);
                ScreenBmp = If_GetScreen(FindHnd);
                if(If_GetPixels(FindHnd, If_SelectColor(), ScreenBmp, pPos.Dig_X, pPos.Dig_Y, nCurNum))
                {
                    If_SearchImg(ScreenBmp, Confirm_Img, pPos.Dig_X, pPos.Dig_Y);
                }
            }
            else if (nCurNum<=0)
            {
                MessageBox.Show("장수를 선택하세요");
            }
            else if (comboBox2.SelectedIndex < 0)
            {
                MessageBox.Show("색을 선택하세요");
            }
            else
            {
                MessageBox.Show("창을 찾을 수 없습니다.");
            }
        }
        

        private Boolean If_FindHandle(IntPtr handle)
        {
            
            if(FindHnd != IntPtr.Zero)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void If_GetHandlePos(IntPtr hnd)
        {
            System.Drawing.Point point = new System.Drawing.Point();
            System.Drawing.Size size = new System.Drawing.Size();

            GetWindowPos(hnd, ref point, ref size);

            pPos.Dig_X = point.X;
            pPos.Dig_Y = point.Y;
        }

        private void GetWindowPos(IntPtr hwn, ref System.Drawing.Point point, ref System.Drawing.Size size)
        {
            WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
            placement.length = System.Runtime.InteropServices.Marshal.SizeOf(placement);

            GetWindowPlacement(hwn, ref placement);

            size = new System.Drawing.Size(placement.normal_position.Right - (placement.normal_position.Left * 2), 
                                            placement.normal_position.Bottom - (placement.normal_position.Top*2));

            point = new System.Drawing.Point(placement.normal_position.Left, placement.normal_position.Top);
        }

        private Bitmap If_GetScreen(IntPtr handle)
        {
            Graphics GraphicsData = Graphics.FromHwnd(handle);
            Rectangle rect = Rectangle.Round(GraphicsData.VisibleClipBounds);
            Bitmap bmp = new Bitmap(rect.Width, rect.Height);

            using (Graphics G = Graphics.FromImage(bmp)) 
            {
                IntPtr hdc = G.GetHdc();
                PrintWindow(handle, hdc, 0x2);
                G.ReleaseHdc(hdc);
            }
            pictureBox1.Image = bmp;
            return bmp;
        }
        private bool If_SearchImg(Bitmap screen_Img, Bitmap find_Img, int pos_x, int pos_y)
        {
            using (Mat ScreenMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(screen_Img))
            using (Mat FindMat = OpenCvSharp.Extensions.BitmapConverter.ToMat(find_Img))

            using(Mat res = ScreenMat.MatchTemplate(FindMat, TemplateMatchModes.CCoeffNormed))
            {
                double minval, maxval = 0;
                OpenCvSharp.Point minloc, maxloc;
                Cv2.MinMaxLoc(res, out minval, out maxval, out minloc, out maxloc);
                Debug.WriteLine("유사도: "+maxval);

                if (maxval >= 0.8)
                {
                    InClick(maxloc.X + pos_x + FindMat.Width/2, maxloc.Y + pos_y + FindMat.Height/2);
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        public void InClick(int x, int y)
        {
            Cursor.Position = new System.Drawing.Point(x, y);
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);
        }
        private void initial()
        {
            comboBox1.Items.Add("1장");
            comboBox1.Items.Add("2장");
            comboBox1.Items.Add("3장");
            comboBox1.Items.Add("4장");

            comboBox2.Items.Add("보라");
            comboBox2.Items.Add("진초록");
            comboBox2.Items.Add("하늘");
            comboBox2.Items.Add("오렌지");
            comboBox2.Items.Add("연두");
            comboBox2.Items.Add("멜론");
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Color c = new Color();

            Rectangle rect = e.Bounds;

            if(e.Index >= 0)
            {
                string n = ((ComboBox)sender).Items[e.Index].ToString();

                Font f = new Font("Arial", 9, FontStyle.Regular);

                switch (n)
                {
                    case "보라":
                        c = Color_Purple;
                        break;

                    case "진초록":
                        c = Color_DrakGreen;
                        break;

                    case "하늘":
                        c = Color_SkyBlue;
                        break;

                    case "오렌지":
                        c = Color_Orange;
                        break;

                    case "연두":
                        c = Color_LightGreen;
                        break;

                    case "멜론":
                        c = Color_Melon;
                        break;
                }

                Brush b = new SolidBrush(c);

                g.FillRectangle(b, rect.X, rect.Y, rect.Width, rect.Height);
                g.DrawString(n, f, Brushes.Black, rect.X, rect.Top);
            }
        }

        private Color If_SelectColor()
        {
            string str = comboBox2.SelectedItem.ToString();

            Color c = new Color();
            switch (str)
            {
                case "보라":
                    c = Color_Purple;
                    break;

                case "진초록":
                    c = Color_DrakGreen;
                    break;

                case "하늘":
                    c = Color_SkyBlue;
                    break;

                case "오렌지":
                    c = Color_Orange;
                    break;

                case "연두":
                    c = Color_LightGreen;
                    break;
                case "멜론":
                    c = Color_Melon;
                    break;
            }

            return c;
        }

        private bool If_GetPixels(IntPtr pHandle, Color Select_color, Bitmap Screen_Img, int nx, int ny, int index)
        {
            Color FindColor;
            int nCx = 0, nCy = 0;
            for(int y=0; y<Screen_Img.Height/8; y++)
            {
                for(int x=0; x<Screen_Img.Width/8; x++)
                {
                    nCx = x * 8;
                    nCy = y * 8;

                    FindColor = Screen_Img.GetPixel(nCx, nCy);

                    if (FindColor == Select_color)
                    {
                        if (index > 0)
                        {
                            InClick(nCx + nx, nCy + ny);
                            index--;
                        }
                        else if (index == 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private void button1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }
    }
}
