using DNStudy.lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DNStudy
{
    public partial class Form1 : Form
    {
        private Point local;

        public static IntPtr osk_main_handle = IntPtr.Zero;
        public static IntPtr osk_handle = IntPtr.Zero;

        public delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);
        List<string> list = new List<string>();
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, IntPtr lpString, int nMaxCount);

        public bool EnumWindowsMethod(IntPtr hWnd, IntPtr lParam)
        {
            IntPtr lpString = Marshal.AllocHGlobal(200);
            GetWindowText(hWnd, lpString, 200);
            osk_handle = hWnd;
            var text = Marshal.PtrToStringAnsi(lpString);
            if (!string.IsNullOrWhiteSpace(text))
                list.Add(text);
            return true;
        }

        public Form1()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Hand.initMap();
            RegisterHotkey();
            Rectangle rect = new Rectangle();
            rect = Screen.GetWorkingArea(this);
            Utils.screenWidth = Screen.PrimaryScreen.Bounds.Width;//屏幕宽
            Utils.screenHeight = Screen.PrimaryScreen.Bounds.Height;//屏幕高

            Process[] process_osk = Process.GetProcessesByName("osk");
            if (process_osk == null || process_osk.Length == 0)
            {
                System.Diagnostics.Process.Start("osk.exe");
                //RunCmd("osk.exe", "");
                Thread.Sleep(3000);
            }
            osk_main_handle = Process.GetProcessesByName("osk")[0].MainWindowHandle;
            list.Clear();
            EnumChildWindows(osk_main_handle, this.EnumWindowsMethod, IntPtr.Zero);


            //osk_handle = (IntPtr)722946;

        }

        static bool RunCmd(string cmdExe, string cmdStr)
        {
            bool result = false;
            try
            {
                using (Process myPro = new Process())
                {
                    //指定启动进程是调用的应用程序和命令行参数
                    ProcessStartInfo psi = new ProcessStartInfo(cmdExe, cmdStr);
                    myPro.StartInfo = psi;
                    myPro.Start();
                    myPro.WaitForExit();
                    result = true;
                }
            }
            catch
            {

            }
            return result;
        }

        private void RegisterHotkey()
        {
            HotKey.RegisterHotKey(Handle, 0xAAAA, HotKey.KeyModifiers.None, Keys.F8);

            HotKey.RegisterHotKey(Handle, 0xAAAB, HotKey.KeyModifiers.None, Keys.F9);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            HotKey.UnregisterHotKey(Handle, 0xAAAA);
            HotKey.UnregisterHotKey(Handle, 0xAAAB);
            isRunning = false;
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_HOTKEY = 0x0312;            //如果m.Msg的值为0x0312那么表示用户按下了热键 
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case 0xAAAA:
                            addPoint();
                            //auto_attack1();
                            break;
                        case 0xAAAB:
                            addPointAndColor();
                            //moveToDongmeng();
                            break;
                    }
                    break;
            }
            base.WndProc(ref m);
        }

        private void addPointAndColor()
        {
            Bitmap bmp = (Bitmap)pictureBox1.Image;
            bmp.Save(textBox_expect_point.Text + " " + textBox_color.Text + ".bmp");
        }

        private void addPoint()
        {
            textBox1.Text += "mlc " + textBox_expect_point.Text + "\r\n";
        }

        private void mouse_test(object p1, object p2)
        {
            Utils.m_moveTo(100, 100);
            Thread.Sleep(500);
            Utils.m_l_click();
            //Utils.m_moveR(100, 100);
        }

        public void moveToDongmeng()
        {
            Hand.click("M");
            Utils.m_moveTo(886, 511);
            Utils.m_l_click();
        }

        public void auto_attack1()
        {
            //KeyMap.clicks("WS2DFFSSDD");
            for (int i = 0; i < 50; i++)
            {
                Hand.PRESSDOWN("D");
                Thread.Sleep(200);
            }

        }
        Boolean isRunning = true;

        private void button1_Click(object sender, EventArgs e)
        {
            isRunning = true;
            new Thread(new ThreadStart(getMousePoint)).Start();
        }


        private void button_stop_Click(object sender, EventArgs e)
        {
            isRunning = false;
        }
        public void getMousePoint()
        {
            while (isRunning)
            {
                int offsetx = 0, offsety = 0;
                try
                {
                    String[] s = textBox_offset.Text.Split(' ');
                    offsetx = int.Parse(s[0]);
                    offsety = int.Parse(s[1]);
                }
                catch (System.Exception ex)
                {
                	
                }
                Point screenPoint = Control.MousePosition;
                String str = screenPoint.X + " " + screenPoint.Y;
                int x1 = screenPoint.X;
                int y1 = screenPoint.Y;
                textBox_point.Text = str;
                textBox_expect_point.Text = (x1 - offsetx) + " " + (y1 - offsety);
                x1 -= 8;
                y1 -= 8;
                x1 = x1 <= 0 ? 0 : x1;
                y1 = y1 <= 0 ? 0 : y1;
                Bitmap bmp = PicUtils.captureScreen(x1, y1, 17, 17);
                int scale = 8;
                Bitmap bmp2 = new Bitmap(bmp.Width * scale, bmp.Height * scale);
                for (int i = 0; i < bmp.Width; i++)
                {
                    for (int j = 0; j < bmp.Height; j++)
                    {
                        for (int k = 0; k < scale; k++)
                        {
                            for (int l = 0; l < scale; l++)
                            {
                                bmp2.SetPixel(i * scale + k, j * scale + l, bmp.GetPixel(i, j));
                            }
                        }
                        if (i == bmp.Width / 2 && j == bmp.Height / 2)
                        {
                            for (int k = 0; k < scale; k++)
                            {
                                for (int l = 0; l < scale; l++)
                                {
                                    if (k == 0 || k == scale - 1 || l == 0 || l == scale - 1)
                                        bmp2.SetPixel(i * scale + k, j * scale + l, Color.Red);
                                    else
                                        bmp2.SetPixel(i * scale + k, j * scale + l, bmp.GetPixel(i, j));
                                }
                            }
                        }
                    }
                }
                Color c = bmp.GetPixel(bmp.Width / 2, bmp.Height / 2);
                textBox_color.Text = c.Name.Substring(2, 6).ToUpper();
                pictureBox1.Image = bmp2;
                Thread.Sleep(100);
            }
        }

        private void setText(TextBox textBox, string str)
        {
            if (textBox.InvokeRequired)
            {

            }
        }

        private void create_Osk_map()
        {
            string s = "`1234567890";
            int x = 80, y = 12;
            String ret = "";
            for (int i = 0; i < s.ToCharArray().Length; i++)
            {
                ret += new string(new char[] { s[i] }) + "=" + (x + i * 40) + ";" + y + ",";
            }
            s = "qwertyuiop";
            x += 20;
            y += 28;
            for (int i = 0; i < s.ToCharArray().Length; i++)
            {
                ret += new string(new char[] { s[i] }) + "=" + (x + i * 40) + ";" + y + ",";
            }
            s = "asdfghjkl";
            x += 20;
            y += 28;
            for (int i = 0; i < s.ToCharArray().Length; i++)
            {
                ret += new string(new char[] { s[i] }) + "=" + (x + i * 40) + ";" + y + ",";
            }
            s = "zxcvbnm";
            x += 20;
            y += 28;
            for (int i = 0; i < s.ToCharArray().Length; i++)
            {
                ret += new string(new char[] { s[i] }) + "=" + (x + i * 40) + ";" + y + ",";
            }

            textBox1.Text = ret;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            activityWindows();
            //for (int i = 0; i < 10; i++)
            //{
            //    Hand.LONG_PRESSDOWN("f");
            //    Thread.Sleep(100);
            //}

            //KeyMap.PRESSUP("w");

            Hand.clicks("1,w890");
            //for (int i = 0; i < 100 ; i++)
            //{

            //    Utils.m_moveR(0, 1);
            //    Thread.Sleep(10);
            //}
            //Hand.moveR(200, 0);
            //MessageBox.Show("complete");
        }

        public void activityWindows()
        {
            Thread.Sleep(1000);
            Utils.m_moveTo_l_click(65, 785);
            Hand.clicks("MM");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            local = new Point(1290, 10);
            this.Location = local;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            activityWindows();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            activityWindows();
            osk_handle = Process.GetProcessesByName("osk")[0].MainWindowHandle;
            osk_handle = (IntPtr)722946;
            Utils.sendMouseClick(osk_handle, 120, 66);

            Utils.sendMousePress(osk_handle, 120, 66);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //List<Point> points = PicUtils.FindPic(65, 290, 360, 650, "C:\\1\\70中级生命龙玉.bmp");
            //for (int i = 0; i < points.Count; i++)
            //{
            //    textBox1.Text = "p" + i + " = " + points[i].X + " , " + points[i].Y + "\r\n";
            //}
            for (int j = 0; j < 10000; j++)
            {
                Script script = new Script();
                script.ScriptStr = "p,,;mlc 420 561;p,,,;mlc 482 668;p,,,;mlc 482 668;p-....;mlc 482 576;p..";
                //script.ScriptStr = content;
                for (int i = 0; i < 8; i++)
                {
                    script.excute();
                }
                script.ScriptStr = "p,,;mlc 921 445;p.;mlc 478 607;p.;mlc 601 333;p,,";
                script.ScriptStr += "p,;mlc 925 231;p--..";
                script.excute();
                script.ScriptStr = "p.;mlc 682 695;p.;mlc 750 365;p.;mlc 750 438;p.;mlc 750 514;p.;mlc 750 600;p..;mlc 818 292;p...";
                script.ScriptStr += "p.;mlc 438 695;p.;mlc 448 365;p.;mlc 448 465;p.;mlc 448 552;p.;mlc 448 650;p..;mlc 818 292;p...";
                script.ScriptStr += "p,,,;mlc 910 411;p--..";
                //910,411
                script.excute();
            }


        }

        private void button6_Click(object sender, EventArgs e)
        {
            string filename = "";
            string content = File.ReadAllText("天梯.txt");

            Script script = new Script();
            script.ScriptStr = "mlc 20 20;p,fdsaf dfjdskfs dfdjk yzy ;mlc 1000 800;p,hfjdkafdjsfdhskds.1fhdjsuwjdhfdshjs;mlc 30 600";
            //script.ScriptStr = content;
            script.excute();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //PicUtils.FindColor(246, 130, 444, 289, "3cc4c4",0.9);
            //PicUtils.FindColor(246, 130, 444, 289, "3cc4c4", 0.93);
            //PicUtils.FindColorFromFile(@"C:\Users\gypc\Pictures\dn\小图\小地图.bmp", "62b040", 0.80, "80.bmp");
            //PicUtils.FindColorFromFile(@"C:\Users\gypc\Pictures\dn\小图\小地图.bmp", "62b040", 0.70, "70.bmp");

            //349E57
            String rgb = "";
            rgb = "62b040";

            rgb = "3CC43C";

            //rgb = "228545";
            List<String> rgbs = new List<String>();
            List<double> sims = new List<double>();

            //PicUtils.FindColorFromFile(@"C:\Users\gypc\Pictures\dn\小图\小地图.bmp", rgb, 0.90, "90.bmp");
            //PicUtils.FindColorFromFile(@"C:\Users\gypc\Pictures\dn\小图\小地图.bmp", rgb, 0.85, "85.bmp");
            //PicUtils.FindColorFromFile(@"C:\Users\gypc\Pictures\dn\小图\小地图.bmp", rgb, 0.80, "80.bmp");

            rgbs.Add("00FF00");
            sims.Add(0.9);
            sims.Add(0.88);
            sims.Add(0.85);
            sims.Add(0.82);
            sims.Add(0.80);

            for (int i = 0; i < rgbs.Count; i++)
            {
                for (int j = 0; j < sims.Count; j++)
                {
                    PicUtils.FindColorByHueFromFile(@"C:\Users\gypc\Pictures\dn\小图\小地图.bmp", rgbs[i], sims[j], "Hue_" + rgbs[i] + sims[j].ToString() + ".bmp");

                    PicUtils.FindColorFromFile(@"C:\Users\gypc\Pictures\dn\小图\小地图.bmp", rgbs[i], sims[j], "RGB_" + rgbs[i] + sims[j].ToString() + ".bmp");
                }
            }
        }

        private void button_execute_Click(object sender, EventArgs e)
        {
            Script script = new Script();
            script.ScriptStr = textBox1.Text;
            //script.ScriptStr = content;
            script.excute();
        }

        private void button_copy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(textBox1.Text);
        }
    }
}
