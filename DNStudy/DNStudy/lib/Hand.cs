using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DNStudy.lib
{
    class Hand
    {
        static String keyarray = "Modifiers=-65536,None=0,LButton=1,RButton=2,Cancel=3,MButton=4,XButton1=5,XButton2=6,Back=8,Tab=9,LineFeed=10,Clear=12,Enter=13,Return=13,ShiftKey=16,ControlKey=17,Menu=18,Pause=19,CapsLock=20,Capital=20,KanaMode=21,HanguelMode=21,HangulMode=21,JunjaMode=23,FinalMode=24,KanjiMode=25,HanjaMode=25,Escape=27,IMEConvert=28,IMENonconvert=29,IMEAceept=30,IMEAccept=30,IMEModeChange=31,Space=32,Prior=33,PageUp=33,Next=34,PageDown=34,End=35,Home=36,Left=37,Up=38,Right=39,Down=40,Select=41,Print=42,Execute=43,PrintScreen=44,Snapshot=44,Insert=45,Delete=46,Help=47,D0=48,D1=49,D2=50,D3=51,D4=52,D5=53,D6=54,D7=55,D8=56,D9=57,A=65,B=66,C=67,D=68,E=69,F=70,G=71,H=72,I=73,J=74,K=75,L=76,M=77,N=78,O=79,P=80,Q=81,R=82,S=83,T=84,U=85,V=86,W=87,X=88,Y=89,Z=90,0=48,1=49,2=50,3=51,4=52,5=53,6=54,7=55,8=56,9=57,a=65,b=66,c=67,d=68,e=69,f=70,g=71,h=72,i=73,j=74,k=75,l=76,m=77,n=78,o=79,p=80,q=81,r=82,s=83,t=84,u=85,v=86,w=87,x=88,y=89,z=90,LWin=91,RWin=92,Apps=93,Sleep=95,NumPad0=96,NumPad1=97,NumPad2=98,NumPad3=99,NumPad4=100,NumPad5=101,NumPad6=102,NumPad7=103,NumPad8=104,NumPad9=105,Multiply=106,Add=107,Separator=108,Subtract=109,Decimal=110,Divide=111,F1=112,F2=113,F3=114,F4=115,F5=116,F6=117,F7=118,F8=119,F9=120,F10=121,F11=122,F12=123,f1=112,f2=113,f3=114,f4=115,f5=116,f6=117,f7=118,f8=119,f9=120,f10=121,f11=122,f12=123,F13=124,F14=125,F15=126,F16=127,F17=128,F18=129,F19=130,F20=131,F21=132,F22=133,F23=134,F24=135,NumLock=144,Scroll=145,LShiftKey=160,RShiftKey=161,LControlKey=162,RControlKey=163,LMenu=164,RMenu=165,BrowserBack=166,BrowserForward=167,BrowserRefresh=168,BrowserStop=169,BrowserSearch=170,BrowserFavorites=171,BrowserHome=172,VolumeMute=173,VolumeDown=174,VolumeUp=175,MediaNextTrack=176,MediaPreviousTrack=177,MediaStop=178,MediaPlayPause=179,LaunchMail=180,SelectMedia=181,LaunchApplication1=182,LaunchApplication2=183,Oem1=186,OemSemicolon=186,Oemplus=187,Oemcomma=188,OemMinus=189,OemPeriod=190,OemQuestion=191,Oem2=191,Oemtilde=192,Oem3=192,Oem4=219,OemOpenBrackets=219,OemPipe=220,Oem5=220,Oem6=221,OemCloseBrackets=221,Oem7=222,OemQuotes=222,Oem8=223,Oem102=226,OemBackslash=226,ProcessKey=229,Packet=231,Attn=246,Crsel=247,Exsel=248,EraseEof=249,Play=250,Zoom=251,NoName=252,Pa1=253,OemClear=254,KeyCode=65535,Shift=65536,Control=131072,Alt=262144";
        static String keyarray1 = "D0=8b,D1=82,D2=83,D3=84,D4=85,D5=86,D6=87,D7=88,D8=89,D9=8a,0=8b,1=82,2=83,3=84,4=85,5=86,6=87,7=88,8=89,9=8a,A=9e,B=b0,C=ae,D=a0,E=92,F=a1,G=a2,H=a3,I=97,J=a4,K=a5,L=a6,M=b2,N=b1,O=98,P=99,Q=90,R=93,S=9f,T=94,U=96,V=af,W=91,X=ad,Y=95,Z=ac,a=9e,b=b0,c=ae,d=a0,e=92,f=a1,g=a2,h=a3,i=97,j=a4,k=a5,l=a6,m=b2,n=b1,o=98,p=99,q=90,r=93,s=9f,t=94,u=96,v=af,w=91,x=ad,y=95,z=ac";
        static String keyarray2 = " =240,120;`=80;12,1=120;12,2=160;12,3=200;12,4=240;12,5=280;12,6=320;12,7=360;12,8=400;12,9=440;12,0=480;12,q=100;40,w=140;40,e=180;40,r=220;40,t=260;40,y=300;40,u=340;40,i=380;40,o=420;40,p=460;40,a=120;68,s=160;68,d=200;68,f=240;68,g=280;68,h=320;68,j=360;68,k=400;68,l=440;68,z=140;96,x=180;96,c=220;96,v=260;96,b=300;96,n=340;96,m=380;96";
        public static Dictionary<String, Byte> VkDic = new Dictionary<String, Byte>();
        public static Dictionary<String, Byte> ScanDic = new Dictionary<String, Byte>();
        public static Dictionary<String, Point> OskDic = new Dictionary<String, Point>();

        public static int default_delay=2000;

        public static void initMap()
        {
            String[] ss = keyarray.Split(',');
            for(int i=0; i<ss.Length;i++)
            {
                String[] s = ss[i].Split('=');
                byte value = 0;
                try
                {
                    value = Convert.ToByte(s[1]);
                }
                catch
                {
                }
                VkDic.Add(s[0],value );
            }

            ss = keyarray1.Split(',');
            for (int i = 0; i < ss.Length; i++)
            {
                String[] s = ss[i].Split('='); 
                byte value = 0;
                try
                {
                    value = Convert.ToByte(s[1]);
                }
                catch
                {
                }
                ScanDic.Add(s[0], value);
            }

            ss = keyarray2.Split(',');
            for (int i = 0; i < ss.Length; i++)
            {
                String[] s = ss[i].Split('=');
                Point value=new Point();
                try
                {
                    string[] sss = s[1].Split(';');
                    value.X = int.Parse(sss[0]);
                    value.Y = int.Parse(sss[1]);
                }
                catch
                {
                }
                OskDic.Add(s[0], value);
            }
        }

        public static void click(String key)
        {
            Utils.keybd_event(vk(key), 0, 0, 0);
            Thread.Sleep(30);
            Utils.keybd_event(vk(key), 0, 2, 0);
            //PRESSDOWN(key);
            Thread.Sleep(default_delay);

            Console.WriteLine(key);
        }

        public static void clicks(String key)
        {
            char[] cs = key.ToCharArray();
            for (int i = 0; i < cs.Length; i++)
            {
                if (cs[i] == ',')
                {
                    Thread.Sleep(1000);
                    continue;
                }
                if (cs[i] == '.')
                {
                    Thread.Sleep(5000);
                    continue;
                }
                Hand.click(new string(new char[] { cs[i] }));
            }
        }

        public static void PRESSDOWN(String key)
        {
            Utils.keybd_event(vk(key), scan(key), 0, 0);
            Console.WriteLine(key);
        }

        public static void LONG_PRESSDOWN(String key)
        {
            Utils.keybd_event(vk(key), scan(key), 1, 0);
            Console.WriteLine(key);
        }

        public static void PRESSDOWN_S(String key)
        {
            char[] cs = key.ToCharArray();
            for (int i = 0; i < cs.Length; i++)
            {
                if (cs[i] == ',')
                {
                    Thread.Sleep(1000);
                    break;
                }
                if (cs[i] == '.')
                {
                    Thread.Sleep(5000);
                    break;
                }
                PRESSDOWN(new string(new char[] { cs[i] }));
            }
        }

        public static void PRESSUP(String key)
        {
            Utils.keybd_event(vk(key), scan(key), 2, 0);
        }

        public static byte vk(string key)
        {
            byte value = 0;
            try
            {
                VkDic.TryGetValue(key, out value);
            }
            catch
            {
            }
            return value;
        }

        public static byte scan(string key)
        {
            byte value = 0;
            try
            {
                ScanDic.TryGetValue(key, out value);
            }
            catch
            {
            }
            return value;
        }

        public static Point oskpoint(string key)
        {
            Point value = new Point();
            try
            {
                OskDic.TryGetValue(key, out value);
            }
            catch
            {
            }
            return value;
        }

        public static void moveR(int x, int y)
        {
            //Point screenPoint = Control.MousePosition
            double pi = Math.PI;
            double ii = 0;
            int total = x/2;
            for (int i = 0; i < total ; i++)
            {
                Utils.m_moveR(x / total + (int)(Math.Cos(ii) * 10), 0);
                Thread.Sleep((int)(200*Math.Cos(pi/2*i/total)));
            }
        }



        public static void osk_click(String key)
        {
            Point p = oskpoint(key);
            Utils.sendMouseClick(Form1.osk_handle, p.X, p.Y);

            Thread.Sleep(default_delay);

            Console.WriteLine(key);
        }

        public static void osk_clicks(String key)
        {
            char[] cs = key.ToCharArray();
            for (int i = 0; i < cs.Length; i++)
            {
                if (cs[i] == ',')
                {
                    Thread.Sleep(1000);
                    continue;
                }
                if (cs[i] == '.')
                {
                    Thread.Sleep(5000);
                    continue;
                }
                if (cs[i] == '-')
                {
                    Thread.Sleep(30000);
                    continue;
                }
                Hand.osk_click(new string(new char[] { cs[i] }));
            }
        }


        public static void moveTo(int x, int y)
        {
            //Utils.m_moveR(1, 1);
            //Thread.Sleep(15);
            Point screenPoint = Control.MousePosition;
            
            int x1 = screenPoint.X;
            int y1 = screenPoint.Y;
            int count=30;
            double delx = (double)(x - x1)/count;
            double dely = (double)(y - y1)/count;
            Console.WriteLine(x);
            Console.WriteLine(y);
            Console.WriteLine(x1);
            Console.WriteLine(y1);
            Console.WriteLine(delx);
            Console.WriteLine(dely);
            Console.WriteLine("--------------------");
            for (int i = 0; i <= count;i++ )
            {
                Utils.m_moveTo(x1 + (int)(delx * i), y1 + (int)(dely * i));
                Thread.Sleep(15);
            }
        }

        public static void moveToLeftClick(int x, int y)
        {
            //Utils.m_moveR(1, 1);
            //Thread.Sleep(15);
            Point screenPoint = Control.MousePosition;

            int x1 = screenPoint.X;
            int y1 = screenPoint.Y;
            int count = 150;
            double delx = (double)(x - x1) / count;
            double dely = (double)(y - y1) / count;
            Console.WriteLine(x);
            Console.WriteLine(y);
            Console.WriteLine(x1);
            Console.WriteLine(y1);
            Console.WriteLine(delx);
            Console.WriteLine(dely);
            Console.WriteLine("--------------------");
            for (int i = 0; i <= count; i++)
            {
                Utils.m_moveTo(x1 + (int)(delx * i), y1 + (int)(dely * i));
                Thread.Sleep(15);
            }
            Thread.Sleep(100);
            Utils.m_l_click();
        }
    }
}
