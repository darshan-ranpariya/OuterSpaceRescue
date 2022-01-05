#if UNITY_STANDALONE && !UNITY_EDITOR
//#if  UNITY_STANDALONE || UNITY_EDITOR
#define WINDOWS_UTILITY
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;
using System.IO;
#if WINDOWS_UTILITY
using Microsoft.Win32;
#endif

public class WindowsUtility : MonoBehaviour
{
#if WINDOWS_UTILITY
    public const bool isAvailable = true;
#else
    public const bool isAvailable = false;
#endif

    public static IntPtr thisWindow;
    public static int _parentWindow = -99;
    public static int parentWindow
    {
        get
        {
            if (_parentWindow == -99)
            {
                int i = -1;
                string[] clArgs = Environment.GetCommandLineArgs();
                if (clArgs != null && clArgs.Length > 5) int.TryParse(clArgs[5], out i);
                _parentWindow = i;
            }
            return _parentWindow;
        }
    }

    bool init = false;
    public bool alwaysOnTop = false;
    public bool fireFakeMouseEvents = false;

    public static event Action SizeChanged;

#if WINDOWS_UTILITY
    private static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
    private static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
    private const UInt32 SWP_NOSIZE = 0x0001;
    private const UInt32 SWP_NOMOVE = 0x0002; 
    private const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;
    private const UInt32 TOP_FLAGS = SWP_NOMOVE;

    private const int ALT = 0xA4;
    private const int EXTENDEDKEY = 0x1;
    private const int KEYUP = 0x2;
     
    private const int MOUSEEVENTF_MOVE = 0x0001;



    [DllImport("user32.dll")]
    private static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    static extern void SwitchToThisWindow(IntPtr hWnd, bool fAltTab);

    [DllImport("user32.dll")]
    private static extern System.IntPtr GetActiveWindow();

    //[DllImport("user32.dll")]
    //public static extern IntPtr FindWindowEx(IntPtr parentWindow, IntPtr previousChildWindow, string windowClass, string windowTitle);

    //[DllImport("user32.dll")]
    //private static extern IntPtr GetWindowThreadProcessId(IntPtr window, out int process);

    [DllImport("user32.dll")]
    private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

    [DllImport("user32.dll")]
    private static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, int dwExtraInfo); 

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);


    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);  
    public struct WINDOWPLACEMENT
    {
        public int length;
        public int flags;
        public int showCmd;
        public System.Drawing.Point ptMinPosition;
        public System.Drawing.Point ptMaxPosition;
        public System.Drawing.Rectangle rcNormalPosition;
    }


    const int WM_COPYDATA = 0x4A;
    //For use with WM_COPYDATA and COPYDATASTRUCT
    [DllImport("User32.dll", EntryPoint = "SendMessage")]
    public static extern int SendMessage(int hWnd, int Msg, int wParam, ref COPYDATASTRUCT lParam);
    public struct COPYDATASTRUCT
    {
        public IntPtr dwData;
        public int cbData;
        [MarshalAs(UnmanagedType.LPStr)]
        public string lpData;
    }


#endif  

    static string _ExecutingPath = "";
    public static string ExecutingPath
    {
        get
        {
            if (string.IsNullOrEmpty(_ExecutingPath))
            {
                _ExecutingPath = Application.dataPath;
                int t = 0;
                for (int i = _ExecutingPath.Length-1; i > 0; i--)
                {
                    //print(_ExecutingPath[i]);
                    t++;
                    if (_ExecutingPath[i].Equals('/') || _ExecutingPath[i].Equals('\\'))
                    {
                        break;
                    }
                }
                _ExecutingPath = _ExecutingPath.Remove(_ExecutingPath.Length - t, t);
            }
            return _ExecutingPath;
        }
    }
    
    static string _registryKey = string.Empty;
    public static string registryKey
    {
        get
        {
#if WINDOWS_UTILITY
            if (string.IsNullOrEmpty(_registryKey)) _registryKey = string.Format("HKEY_CURRENT_USER\\Software\\{0}\\{1}", Application.companyName, Application.productName);
#endif
            return _registryKey;
        }
    } 
    public static void SetRegistry<T>(string key, T value)
    {
#if WINDOWS_UTILITY
        Registry.SetValue(registryKey, key, value);
#endif
    } 
    public static T GetRegistry<T>(string key, T defaultValue)
    {
#if WINDOWS_UTILITY
        return (T)Convert.ChangeType(Registry.GetValue(registryKey, key, defaultValue), typeof(T));
#else
        return default(T);
#endif
    }

    public static bool isEmbeded
    {
        get
        {
            bool isEmbeded = false;
            foreach (string arg in Environment.GetCommandLineArgs())
            {
                if (arg.Equals("-parentHWND"))
                {
                    isEmbeded = true;
                    break;
                }
            }
            return isEmbeded;
        }
    }

    /// <summary>
    /// Opens a new winodow of a program 
    /// </summary>
    /// <param name="exePath">path of executable relative to the ExecutingPath</param>
    public static void OpenWindow(string exePath)
    {
        Application.OpenURL(Path.Combine(ExecutingPath, exePath));
    }

    public void m_SetOnTop(bool set)
    {
        SetOnTop(set);
    }
    public static void SetOnTop(bool set)
    {
#if WINDOWS_UTILITY
        SetWindowPos(thisWindow, set ? HWND_TOPMOST : HWND_BOTTOM, 0, 0, 0, 0, TOPMOST_FLAGS);
#endif
    }

#if WINDOWS_UTILITY
    public static WINDOWPLACEMENT GetWindowPlacement()
    {
        WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
        GetWindowPlacement(thisWindow, ref placement);
        return placement;
    }
#endif

    public void m_BringToForeground()
    {
        BringToForeground();
    }
    public static void BringToForeground()
    {
        BringToForeground(4, true);
    }
    public static void BringToForeground(int mode, bool alt)
    {
#if WINDOWS_UTILITY
        WINDOWPLACEMENT wp = GetWindowPlacement();
        switch (wp.showCmd)
        {
            case 1:
                //Console.WriteLine("Normal");
                break;
            case 2:
                //Console.WriteLine("Minimized");
                ShowWindow(thisWindow, mode);
                break;
            case 3:
                //Console.WriteLine("Maximized");
                break;
        }
        if (alt)
        {
            keybd_event((byte)ALT, 0x45, EXTENDEDKEY | 0, 0);
            keybd_event((byte)ALT, 0x45, EXTENDEDKEY | KEYUP, 0);
        }
        //SwitchToThisWindow(thisWindow, true); 
        SetForegroundWindow(thisWindow);
#endif
    }

    public static int SendMessageToParent(int msgCode, string msg)
    {
#if WINDOWS_UTILITY
        if (parentWindow >= 0)
        {
            byte[] sarr = System.Text.Encoding.Default.GetBytes(msg);
            int len = sarr.Length;
            COPYDATASTRUCT cds;
            cds.dwData = (IntPtr)100;
            cds.lpData = msg;
            cds.cbData = len + 1;
            return SendMessage(parentWindow, WM_COPYDATA, msgCode, ref cds);
        }
#endif
        return -1;
    }


    void OnApplicationFocus(bool hasFocus)
    {
#if WINDOWS_UTILITY
        if (!init && hasFocus)
        {
            thisWindow = GetActiveWindow();
            if (alwaysOnTop) SetOnTop(true);
            if(fireFakeMouseEvents) StartCoroutine("SynMEvt");
            //Application.targetFrameRate = 45; 
        }
#endif
        //Toast.Show();
    }

    int lw, lh;
    void Update()
    {
        if (Screen.height != lh || Screen.width != lw)
        {
            if (SizeChanged != null) SizeChanged();
        }

        lw = Screen.width;
        lh = Screen.height;
    }

    IEnumerator SynMEvt() //Synthetic mouse event
    {
#if WINDOWS_UTILITY
        while (true)
        {
            if (!Application.isFocused)
            {
                mouse_event(MOUSEEVENTF_MOVE, 0, 0, 0, 0);
            }
            yield return new WaitForSeconds(1);
        }
#endif
        yield break;
    }

    public void Test()
    {
        print("clArgs : " + Environment.GetCommandLineArgs().GetDump());
        print("pw : " + parentWindow);
        print("Sending Message");
        int res = SendMessageToParent(111, "OneOneOne");
        print(res);
    }
}
