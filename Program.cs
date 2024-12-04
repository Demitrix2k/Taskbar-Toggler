using System;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;


class Program
{
    static void Main()
    {
        var trayWindow = FindWindow("Shell_TrayWnd", null);

        // Toggle the taskbar's visibility
        if (IsWindowVisible(trayWindow))
        {
            ToggleTaskbarVisibility(trayWindow, true);
            Console.WriteLine("Taskbar disabled.");
        }
        else
        {
            ToggleTaskbarVisibility(trayWindow, false);
            Console.WriteLine("Taskbar enabled.");
        }
    }

    static void ToggleTaskbarVisibility(IntPtr hWnd, bool hide)
    {
        if (hWnd != IntPtr.Zero)  // Check if FindWindow succeeded
        {
            // Set the taskbar to auto-hide using SHAppBarMessage
            if (hide)
            {
                var appBarData = new APPBARDATA
                {
                    cbSize = Marshal.SizeOf(typeof(APPBARDATA)),
                    hWnd = hWnd,
                    uCallbackMessage = 0,
                    uEdge = 0,
                    rc = new Rectangle(),
                    lParam = ABS_AUTOHIDE
                };

                SHAppBarMessage(ABM_SETSTATE, ref appBarData);
                ShowWindow(hWnd, SW_HIDE);
            }
            else
            {
                // Show the taskbar
                ShowWindow(hWnd, SW_SHOW);
            }
        }
    }

    #region winuser.h, shellapi.h
    [StructLayout(LayoutKind.Sequential)]
    struct APPBARDATA
    {
        public int cbSize;
        public IntPtr hWnd;
        public uint uCallbackMessage;
        public uint uEdge;
        public Rectangle rc;
        public int lParam;
    }

    const int SW_HIDE = 0;
    const int SW_SHOW = 5;

    const int ABM_SETSTATE = 0x0000000a;
    const int ABS_AUTOHIDE = 0x00000001;

    [DllImport("user32")]
    static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    [DllImport("user32")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    [DllImport("user32")]
    static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("shell32.dll")]
    static extern bool SHAppBarMessage(int dwMessage, ref APPBARDATA pData);
    #endregion
}