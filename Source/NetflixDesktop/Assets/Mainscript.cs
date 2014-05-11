using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;



public class Mainscript : MonoBehaviour 
{
    IntPtr chromeHandle;

    public enum MouseEventFlags : uint
    {
    LEFTDOWN   = 0x00000002,
    LEFTUP     = 0x00000004,
    MIDDLEDOWN = 0x00000020,
    MIDDLEUP   = 0x00000040,
    MOVE       = 0x00000001,
    ABSOLUTE   = 0x00008000,
    RIGHTDOWN  = 0x00000008,
    RIGHTUP    = 0x00000010,
    WHEEL      = 0x00000800,
    XDOWN      = 0x00000080,
    XUP    = 0x00000100
    }

    string textDisplayDesktop = "";
    string textDisplayMobile = "";
    bool fullScreen = true;

    //Use the values of this enum for the 'dwData' parameter
    //to specify an X button when using MouseEventFlags.XDOWN or
    //MouseEventFlags.XUP for the dwFlags parameter.
    public enum MouseEventDataXButtons : uint
    {
    XBUTTON1   = 0x00000001,
    XBUTTON2   = 0x00000002
    }

    void Awake()
    {
        networkView.group = 1;
    }



	// Use this for initialization
	void Start () 
    {
        UnityEngine.Screen.SetResolution(250, 150, false);
        Network.InitializeServer(3, 25002, true);
        MasterServer.RegisterHost("NetflixSpacebar", "PC", "");
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (chromeHandle == IntPtr.Zero)
        {
            chromeHandle = (IntPtr)FindWindow("Chrome_WidgetWin_1", "Netflix - Google Chrome");
            textDisplayDesktop = "Chrome is not running.";
            return;
        }
        else
        {
            textDisplayDesktop = "Connected to Chrome.";
        }
	}

    void OnGUI()
    {
        if (GUILayout.Button("Test Chrome"))
        {
            SendSpacebarToApp();   
        }
         
        fullScreen = GUILayout.Toggle(fullScreen, "Full Screen");
        GUILayout.Label("Desktop Status: "+textDisplayDesktop);
        GUILayout.Label("Mobile Status: " + textDisplayMobile);
        if (Network.connections.Length == 0)
        {
            textDisplayMobile = "No connections.";
        } else {
            textDisplayMobile = "Connection made.";
        }
    }

    // Get a handle to an application window.
    [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
    public static extern IntPtr FindWindow(string lpClassName,string lpWindowName);

    [DllImport("USER32.DLL")]
    public static extern IntPtr GetForegroundWindow();

    // Activate an application window.
    [DllImport("USER32.DLL")]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

    [RPC]
    public void SendSpacebarToApp()
    {
        chromeHandle = (IntPtr)FindWindow("Chrome_WidgetWin_1", "Netflix - Google Chrome");
 
        if (chromeHandle == IntPtr.Zero)
        {
            textDisplayDesktop = "Netflix not running or timed out?";
            return;
        }

        if (GetForegroundWindow() != chromeHandle)
        {
            SetForegroundWindow(chromeHandle);
        }

        //SwitchToThisWindow(chromeHandle, false);
        System.Windows.Forms.Cursor.Position = new System.Drawing.Point(250, 250);
        mouse_event((int)(MouseEventFlags.LEFTDOWN), 0, 0, 0, 0);
        SendKeys.SendWait(" ");
        if (fullScreen)
        {
            SendKeys.SendWait("f");
        }
        textDisplayDesktop = "Sent successfully";
    }
}



