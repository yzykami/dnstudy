using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DNStudy.lib
{
    class PicUtils
    {

        #region 抓取屏幕
        /// <summary>
        /// 抓取屏幕(层叠的窗口)
        /// </summary>
        /// <param name="x">左上角的横坐标</param>
        /// <param name="y">左上角的纵坐标</param>
        /// <param name="width">抓取宽度</param>
        /// <param name="height">抓取高度</param>
        /// <returns></returns>
        public static Bitmap captureScreen(int x, int y, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(new Point(x, y), new Point(0, 0), bmp.Size);
                g.Dispose();
            }
            //bit.Save(@"capture2.png");
            return bmp;
        }

        /// <summary>
        /// 抓取整个屏幕
        /// </summary>
        /// <returns></returns>
        public static Bitmap captureScreen()
        {
            Size screenSize = Screen.PrimaryScreen.Bounds.Size;
            return captureScreen(0, 0, screenSize.Width, screenSize.Height);
        }
        #endregion

        #region 使用BitBlt方法抓取控件，无论控件是否被遮挡
        /// <summary>
        /// 控件(窗口)的截图，控件被其他窗口(而非本窗口内控件)遮挡时也可以正确截图，使用BitBlt方法
        /// </summary>
        /// <param name="control">需要被截图的控件</param>
        /// <returns>该控件的截图，控件被遮挡时也可以正确截图</returns>
        public static Bitmap captureControl(Control control)
        {
            //调用API截屏
            IntPtr hSrce = GetWindowDC(control.Handle);
            IntPtr hDest = CreateCompatibleDC(hSrce);
            IntPtr hBmp = CreateCompatibleBitmap(hSrce, control.Width, control.Height);
            IntPtr hOldBmp = SelectObject(hDest, hBmp);
            if (BitBlt(hDest, 0, 0, control.Width, control.Height, hSrce, 0, 0, CopyPixelOperation.SourceCopy | CopyPixelOperation.CaptureBlt))
            {
                Bitmap bmp = Image.FromHbitmap(hBmp);
                SelectObject(hDest, hOldBmp);
                DeleteObject(hBmp);
                DeleteDC(hDest);
                ReleaseDC(control.Handle, hSrce);
                // bmp.Save(@"a.png");
                // bmp.Dispose();
                return bmp;
            }
            return null;

        }

        // /// <summary>
        // /// 有问题！！！！！用户区域坐标不对啊
        // /// 控件(窗口)的用户区域截图，控件被其他窗口(而非本窗口内控件)遮挡时也可以正确截图，使用BitBlt方法
        // /// </summary>
        // /// <param name="control">需要被截图的控件</param>
        // /// <returns>控件(窗口)的用户区域截图</returns>
        // public static Bitmap captureClientArea(Control control)
        // {
        // 
        // Size sz = control.Size;
        // Rectangle rect = control.ClientRectangle;
        // 
        // 
        // //调用API截屏
        // IntPtr hSrce = GetWindowDC(control.Handle);
        // IntPtr hDest = CreateCompatibleDC(hSrce);
        // IntPtr hBmp = CreateCompatibleBitmap(hSrce, rect.Width, rect.Height);
        // IntPtr hOldBmp = SelectObject(hDest, hBmp);
        // if (BitBlt(hDest, 0, 0, rect.Width, rect.Height, hSrce, rect.X, rect.Y, CopyPixelOperation.SourceCopy | CopyPixelOperation.CaptureBlt))
        // {
        // Bitmap bmp = Image.FromHbitmap(hBmp);
        // SelectObject(hDest, hOldBmp);
        // DeleteObject(hBmp);
        // DeleteDC(hDest);
        // ReleaseDC(control.Handle, hSrce);
        // // bmp.Save(@"a.png");
        // // bmp.Dispose();
        // return bmp;
        // }
        // return null;
        // 
        // }
        #endregion


        #region 使用PrintWindow方法抓取窗口，无论控件是否被遮挡
        /// <summary>
        /// 窗口的截图，窗口被遮挡时也可以正确截图，使用PrintWindow方法
        /// </summary>
        /// <param name="control">需要被截图的窗口</param>
        /// <returns>窗口的截图，控件被遮挡时也可以正确截图</returns>
        public static Bitmap captureWindowUsingPrintWindow(Form form)
        {
            return GetWindow(form.Handle);
        }


        private static Bitmap GetWindow(IntPtr hWnd)
        {
            IntPtr hscrdc = GetWindowDC(hWnd);
            Control control = Control.FromHandle(hWnd);
            IntPtr hbitmap = CreateCompatibleBitmap(hscrdc, control.Width, control.Height);
            IntPtr hmemdc = CreateCompatibleDC(hscrdc);
            SelectObject(hmemdc, hbitmap);
            PrintWindow(hWnd, hmemdc, 0);
            Bitmap bmp = Bitmap.FromHbitmap(hbitmap);
            DeleteDC(hscrdc);//删除用过的对象
            DeleteDC(hmemdc);//删除用过的对象
            return bmp;
        }
        #endregion

        #region DLL calls
        [DllImport("gdi32.dll")]
        static extern bool BitBlt(IntPtr hdcDest, int xDest, int yDest, int
        wDest, int hDest, IntPtr hdcSource, int xSrc, int ySrc, CopyPixelOperation rop);
        [DllImport("gdi32.dll")]
        static extern IntPtr DeleteDC(IntPtr hDc);
        [DllImport("gdi32.dll")]
        static extern IntPtr DeleteObject(IntPtr hDc);
        [DllImport("gdi32.dll")]
        static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);
        [DllImport("gdi32.dll")]
        static extern IntPtr CreateCompatibleDC(IntPtr hdc);
        [DllImport("gdi32.dll")]
        static extern IntPtr SelectObject(IntPtr hdc, IntPtr bmp);
        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr ptr);
        [DllImport("user32.dll")]
        public static extern bool PrintWindow(IntPtr hwnd, IntPtr hdcBlt, UInt32 nFlags);
        [DllImport("user32.dll")]
        static extern bool ReleaseDC(IntPtr hWnd, IntPtr hDc);
        #endregion


        public static List<Point> FindPic(int x1, int y1, int x2, int y2, String filename)
        {
            List<Point> points = new List<Point>();

            try
            {
                //int r1 = (int)int.Parse(rgb.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                //int g1 = (int)int.Parse(rgb.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                //int b1 = (int)int.Parse(rgb.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

                Bitmap bmp = new Bitmap(filename);

                Bitmap screen = captureScreen(x1, y1, x2 - x1, y2 - y1);

                screen.Save(DateTime.Now.ToFileTime().ToString() + ".bmp");

                for (int i = 0; i < screen.Width - bmp.Width; i++)
                {
                    for (int j = 0; j < screen.Height - bmp.Height; j++)
                    {

                        bool isSame = true;
                        for (int ii = 2; ii < bmp.Width; ii += 4)
                        {
                            for (int jj = 2; jj < bmp.Height; jj += 4)
                            {
                                Color color = screen.GetPixel(i + ii, j + jj);
                                int r2 = color.R;
                                int g2 = color.G;
                                int b2 = color.B;

                                Color color1 = screen.GetPixel(ii, jj);
                                int r1 = color1.R;
                                int g1 = color1.G;
                                int b1 = color1.B;
                                if (r1 == r2 && g1 == g2 && b1 == b2)
                                {
                                    continue;
                                }
                                else
                                {
                                    isSame = false;
                                    break;
                                }
                            }
                        }
                        if (isSame)
                        {
                            Point p = new Point(i, j);
                        }
                    }
                }

            }
            catch (System.Exception ex)
            {

            }


            return points;
        }


        public static List<Point> FindColor(int x1, int y1, int x2, int y2, String rgb,double sim)
        {
            List<Point> points = new List<Point>();

            int ddd = 255;
            double maxdistance = Math.Sqrt(ddd * ddd * 3);
            try
            {
                int r1 = (int)int.Parse(rgb.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                int g1 = (int)int.Parse(rgb.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                int b1 = (int)int.Parse(rgb.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

                //Bitmap bmp = new Bitmap(filename);

                Bitmap screen = captureScreen(x1, y1, x2 - x1, y2 - y1);

                screen.Save(DateTime.Now.ToFileTime().ToString() + ".bmp");

                for (int i = 0; i < screen.Width; i++)
                {
                    for (int j = 0; j < screen.Height; j++)
                    {
                        Color color = screen.GetPixel(i, j);
                        int r2 = color.R;
                        int g2 = color.G;
                        int b2 = color.B;

                        int absR = r1 - r2;
                        int absG = g1 - g2;
                        int absB = b1 - b2;
                        double distance = Math.Sqrt(absR * absR + absG * absG + absB * absB);
                        if ((maxdistance - distance) / maxdistance > sim && distance != 0)
                        {
                            Point p = new Point();
                            p.X = i;
                            p.Y = j;
                            points.Add(p);
                            screen.SetPixel(i, j, Color.Black);
                        }
                    }
                }

                screen.Save(DateTime.Now.ToFileTime().ToString() + ".bmp");

            }
            catch (System.Exception ex)
            {

            }


            return points;
        }


        public static List<Point> FindColorFromFile(String filename, String rgb, double sim,string outfile)
        {
            List<Point> points = new List<Point>();

            int ddd = 255;
            double maxdistance = Math.Sqrt(ddd * ddd * 3);
            try
            {
                int r1 = (int)int.Parse(rgb.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                int g1 = (int)int.Parse(rgb.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                int b1 = (int)int.Parse(rgb.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

                Bitmap screen = new Bitmap(filename);

                //Bitmap screen = captureScreen(x1, y1, x2 - x1, y2 - y1);

                //screen.Save(DateTime.Now.ToFileTime().ToString() + ".bmp");

                for (int i = 0; i < screen.Width; i++)
                {
                    for (int j = 0; j < screen.Height; j++)
                    {
                        Color color = screen.GetPixel(i, j);
                        int r2 = color.R;
                        int g2 = color.G;
                        int b2 = color.B;

                        int absR = r1 - r2;
                        int absG = g1 - g2;
                        int absB = b1 - b2;
                        double distance = Math.Sqrt(absR * absR + absG * absG + absB * absB);
                        if ((maxdistance - distance) / maxdistance > sim )//&& distance != 0)
                        {
                            Point p = new Point();
                            p.X = i;
                            p.Y = j;
                            points.Add(p);
                            screen.SetPixel(i, j, Color.White);
                        }
                        else //if((i>0&&i<80)||(i>120&&i<180)||(j>0&&j<90)||(j>120&&j<200))
                            screen.SetPixel(i, j, Color.Black);
                    }
                }
                if (outfile == "")
                    outfile = "1.bmp";
                screen.Save(outfile);//DateTime.Now.ToFileTime().ToString() + ".bmp");

            }
            catch (System.Exception ex)
            {

            }


            return points;
        }


        public static List<Point> FindColorByHueFromFile(String filename, String rgb, double sim, string outfile)
        {
            List<Point> points = new List<Point>();

            int ddd = 255;
            double maxdistance = Math.Sqrt(120*120);
            Color cc =ColorTranslator.FromHtml("#"+rgb);

            try
            {
                int b1 = (int)int.Parse(rgb.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                int g1 = (int)int.Parse(rgb.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                int r1 = (int)int.Parse(rgb.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

                //int r1 = (int)int.Parse(rgb.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                //int g1 = (int)int.Parse(rgb.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                //int b1 = (int)int.Parse(rgb.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

                Bitmap screen = new Bitmap(filename);

                //Bitmap screen = captureScreen(x1, y1, x2 - x1, y2 - y1);

                //screen.Save(DateTime.Now.ToFileTime().ToString() + ".bmp");

                for (int i = 0; i < screen.Width; i++)
                {
                    for (int j = 0; j < screen.Height; j++)
                    {
                        Color color = screen.GetPixel(i, j);
                        //int r2 = color.R;
                        //int g2 = color.G;
                        //int b2 = color.B;

                        //int absR = r1 - r2;
                        //int absG = g1 - g2;
                        //int absB = b1 - b2;
                        double h1 = cc.GetHue();
                        double h2 = color.GetHue();
                        double del = color.GetHue() - cc.GetHue();
                        double distance = Math.Sqrt(del*del);

                        if ((maxdistance - distance) / maxdistance > sim && distance != 0)
                        {
                            Point p = new Point();
                            p.X = i;
                            p.Y = j;
                            points.Add(p);
                            screen.SetPixel(i, j, Color.White);
                        }
                        else
                            screen.SetPixel(i, j, Color.Black);
                    }
                }
                if (outfile == "")
                    outfile = "1.bmp";
                screen.Save(outfile);//DateTime.Now.ToFileTime().ToString() + ".bmp");

            }
            catch (System.Exception ex)
            {

            }


            return points;
        }
    }
}
