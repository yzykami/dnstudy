using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DNStudy.lib
{
    class Script
    {
        int defaultdelay = 10;
        String scriptStr = "";
        public System.String ScriptStr
        {
            get { return scriptStr; }
            set { scriptStr = value; }
        }
        String[] scripts;

        /*
         * mt = 鼠标移动 ： mt 0 0;
         * mr = 鼠标相对移动： mr 0 10;
         * mlc = 鼠标点击： mlc 0 0;
         * p = 键盘按下
         * ,默认停顿1秒
         * .默认停顿5秒
         */
        
        public void excute()
        {
            scriptStr = scriptStr.Replace(";", "\r\n");
            scripts = scriptStr.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < scripts.Length ; i++)
            {
                String str = scripts[i];
                if (str == "")
                    continue;
                if (str.StartsWith("/"))
                    continue;
                if (str.EndsWith(";"))
                    str = str.Substring(0, str.Length - 1);

                if (str.StartsWith("m"))
                {
                    string[] ss = str.Split(' ');
                    String action = ss[0];
                    int x=1280/2;
                    int y=768/2;
                    int.TryParse(ss[1],out x);
                    int.TryParse(ss[2],out y);
                    if (action == "mlc")
                    {
                        Hand.moveToLeftClick(x, y);
                        Thread.Sleep(200);
                    }

                }
                else if (str.StartsWith("p"))
                {
                    str = str.Substring(1, str.Length - 1);
                    Hand.osk_clicks(str);
                }
            }
        }
    }
}
