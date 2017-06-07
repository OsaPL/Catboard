using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WMPLib; //reference,odwolania

namespace catboard
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var kh = new KeyboardHook(true); //keyboard hook z klasy ponizej
            kh.KeyDown += Kh_KeyDown;
            kh.KeyUp += Kh_KeyUp;
            notifyIcon1.Icon = this.Icon;
            soundsList = new List<string>();
            childopen = false;
            if (!System.IO.File.Exists("Sounds\\CustomSounds.dat"))
            {
                soundsList.Add("Sounds\\meow.mp3");
                soundsList.Add("Sounds\\meow1.mp3");
                soundsList.Add("Sounds\\woof.mp3");
                soundsList.Add("Sounds\\woof2.mp3");
            }
            else
            {
                string filepath = "Sounds\\CustomSounds.dat";
                System.IO.FileInfo file = new System.IO.FileInfo(filepath);
                file.Directory.Create(); // if the directory already exists, this method does nothing, just a failsafe
                string[] settings = System.IO.File.ReadAllLines(file.FullName, Encoding.UTF8);
                int i = 0;  
                while (settings[i] != "")
                {
                    if (settings[i] == "Random")
                    {
                        radioButtonRand.Checked = true;
                    }
                    else if (settings[i] == "Fixed")
                    {
                        radioButtonFix.Checked = true;
                    }
                    else
                    {
                        soundsList.Add(settings[i]);
                    }

                    i++;
                }
            }
        }
        public List<string> soundsList;
        Keys lastkey;
        private void Kh_KeyDown(Keys key, bool Shift, bool Ctrl, bool Alt)//musi byc static??
        {
            label.Text = "Pressed: " + key + " (HOLD)";
            if(lastkey != key)
                Form1_KeyPress(this, null);
            lastkey = key;
        }
        private void Kh_KeyUp(Keys key, bool Shift, bool Ctrl, bool Alt)//musi byc static??
        {
            label.Text = label.Text.Replace("(HOLD)", "");
            lastkey = Keys.None;
        }
        public static int RandNumber(int Low, int High) //lepsze liczby losowe
        {
            Random rndNum = new Random(int.Parse(Guid.NewGuid().ToString().Substring(0, 8), System.Globalization.NumberStyles.HexNumber));

            int rnd = rndNum.Next(Low, High);

            return rnd;
        }
        int last=0;
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            WMPLib.WindowsMediaPlayer wplayer = new WMPLib.WindowsMediaPlayer(); //odtwarzacz mp3 w tle
            if (radioButtonRand.Checked)
            {
                int tmp = last;
                if (soundsList.Count > 1)
                while (tmp == last) //by ominac powtarzanie sie tych samych dzwiekow pod rzad
                    last = RandNumber(0, 500 * soundsList.Count) % soundsList.Count;

                int i = 0;
                foreach (string strings in soundsList)
                {
                    if (strings != "") {
                        if (last == i)
                            wplayer.URL = strings;
                        i++;
                    }

                }
            }
            else
            {

            }
            wplayer.controls.play();
        }
        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == System.Windows.Forms.MouseButtons.Left)
                Activate();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        public SoundsForm soundsform;
        public bool childopen;
        private void SoundsButton_Click(object sender, EventArgs e)
        {
            childopen = true;
            if (radioButtonRand.Checked)
            {
                soundsform = new SoundsForm(this);
                soundsform.Show();
            }
            else
            {
                //tutaj forma z fixed od klaudii
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string[] settings = { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            //cause Im dumb and also lazy ^ DONT LOOK AT THAT LINE ^
            int i;
            for (i = 0; i < soundsList.Count; i++)
            {
                settings[i] = soundsList[i].ToString();
            }
            if (radioButtonFix.Checked)
            {
                settings[i] = "Fixed";
            }
            else
            {
                settings[i] = "Random";
            }
            string filepath = "Sounds\\CustomSounds.dat";
            System.IO.FileInfo file = new System.IO.FileInfo(filepath);
            file.Directory.Create();
            System.IO.File.WriteAllLines(file.FullName, settings, Encoding.UTF8);
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            if (childopen == true)
            {
                soundsform.Focus();
            }
        }
    }
    public class KeyboardHook : IDisposable
    {
        bool Global = false;

        public delegate void LocalKeyEventHandler(Keys key, bool Shift, bool Ctrl, bool Alt);
        public event LocalKeyEventHandler KeyDown;
        public event LocalKeyEventHandler KeyUp;

        public delegate int CallbackDelegate(int Code, int W, int L);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct KBDLLHookStruct
        {
            public Int32 vkCode;
            public Int32 scanCode;
            public Int32 flags;
            public Int32 time;
            public Int32 dwExtraInfo;
        }

        [DllImport("user32", CallingConvention = CallingConvention.StdCall)]
        private static extern int SetWindowsHookEx(HookType idHook, CallbackDelegate lpfn, int hInstance, int threadId);

        [DllImport("user32", CallingConvention = CallingConvention.StdCall)]
        private static extern bool UnhookWindowsHookEx(int idHook);

        [DllImport("user32", CallingConvention = CallingConvention.StdCall)]
        private static extern int CallNextHookEx(int idHook, int nCode, int wParam, int lParam);

        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern int GetCurrentThreadId();

        public enum HookType : int
        {
            WH_JOURNALRECORD = 0,
            WH_JOURNALPLAYBACK = 1,
            WH_KEYBOARD = 2,
            WH_GETMESSAGE = 3,
            WH_CALLWNDPROC = 4,
            WH_CBT = 5,
            WH_SYSMSGFILTER = 6,
            WH_MOUSE = 7,
            WH_HARDWARE = 8,
            WH_DEBUG = 9,
            WH_SHELL = 10,
            WH_FOREGROUNDIDLE = 11,
            WH_CALLWNDPROCRET = 12,
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14
        }

        private int HookID = 0;
        CallbackDelegate TheHookCB = null;

        //Start hook
        public KeyboardHook(bool Global)
        {
            this.Global = Global;
            TheHookCB = new CallbackDelegate(KeybHookProc);
            if (Global)
            {
                HookID = SetWindowsHookEx(HookType.WH_KEYBOARD_LL, TheHookCB,
                    0, //0 for local hook. eller hwnd til user32 for global
                    0); //0 for global hook. eller thread for hooken
            }
            else
            {
                HookID = SetWindowsHookEx(HookType.WH_KEYBOARD, TheHookCB,
                    0, //0 for local hook. or hwnd to user32 for global
                    GetCurrentThreadId()); //0 for global hook. or thread for the hook
            }
        }

        bool IsFinalized = false;
        ~KeyboardHook()
        {
            if (!IsFinalized)
            {
                UnhookWindowsHookEx(HookID);
                IsFinalized = true;
            }
        }
        public void Dispose()
        {
            if (!IsFinalized)
            {
                UnhookWindowsHookEx(HookID);
                IsFinalized = true;
            }
        }

        //The listener that will trigger events
        private int KeybHookProc(int Code, int W, int L)
        {
            KBDLLHookStruct LS = new KBDLLHookStruct();
            if (Code < 0)
            {
                return CallNextHookEx(HookID, Code, W, L);
            }
            try
            {
                if (!Global)
                {
                    if (Code == 3)
                    {
                        IntPtr ptr = IntPtr.Zero;

                        int keydownup = L >> 30;
                        if (keydownup == 0)
                        {
                            if (KeyDown != null) KeyDown((Keys)W, GetShiftPressed(), GetCtrlPressed(), GetAltPressed());
                        }
                        if (keydownup == -1)
                        {
                            if (KeyUp != null) KeyUp((Keys)W, GetShiftPressed(), GetCtrlPressed(), GetAltPressed());
                        }
                        //System.Diagnostics.Debug.WriteLine("Down: " + (Keys)W);
                    }
                }
                else
                {
                    KeyEvents kEvent = (KeyEvents)W;

                    Int32 vkCode = Marshal.ReadInt32((IntPtr)L); //Leser vkCode som er de første 32 bits hvor L peker.

                    if (kEvent != KeyEvents.KeyDown && kEvent != KeyEvents.KeyUp && kEvent != KeyEvents.SKeyDown && kEvent != KeyEvents.SKeyUp)
                    {
                    }
                    if (kEvent == KeyEvents.KeyDown || kEvent == KeyEvents.SKeyDown)
                    {
                        if (KeyDown != null) KeyDown((Keys)vkCode, GetShiftPressed(), GetCtrlPressed(), GetAltPressed());
                    }
                    if (kEvent == KeyEvents.KeyUp || kEvent == KeyEvents.SKeyUp)
                    {
                        if (KeyUp != null) KeyUp((Keys)vkCode, GetShiftPressed(), GetCtrlPressed(), GetAltPressed());
                    }
                }
            }
            catch (Exception)
            {
                //Ignore all errors...
            }

            return CallNextHookEx(HookID, Code, W, L);

        }

        public enum KeyEvents
        {
            KeyDown = 0x0100,
            KeyUp = 0x0101,
            SKeyDown = 0x0104,
            SKeyUp = 0x0105
        }

        [DllImport("user32.dll")]
        static public extern short GetKeyState(System.Windows.Forms.Keys nVirtKey);

        public static bool GetCapslock()
        {
            return Convert.ToBoolean(GetKeyState(System.Windows.Forms.Keys.CapsLock)) & true;
        }
        public static bool GetNumlock()
        {
            return Convert.ToBoolean(GetKeyState(System.Windows.Forms.Keys.NumLock)) & true;
        }
        public static bool GetScrollLock()
        {
            return Convert.ToBoolean(GetKeyState(System.Windows.Forms.Keys.Scroll)) & true;
        }
        public static bool GetShiftPressed()
        {
            int state = GetKeyState(System.Windows.Forms.Keys.ShiftKey);
            if (state > 1 || state < -1) return true;
            return false;
        }
        public static bool GetCtrlPressed()
        {
            int state = GetKeyState(System.Windows.Forms.Keys.ControlKey);
            if (state > 1 || state < -1) return true;
            return false;
        }
        public static bool GetAltPressed()
        {
            int state = GetKeyState(System.Windows.Forms.Keys.Menu);
            if (state > 1 || state < -1) return true;
            return false;
        }
    }
}
