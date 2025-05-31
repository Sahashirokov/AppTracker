using System;
using System.Runtime.InteropServices;
using LauncherApp.Services.MonitorService.Interface;

namespace LauncherApp.Services.MonitorService;

public class WindowEnumerator : IWindowEnumerator
{
    [DllImport("user32.dll")]
    private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);
    
    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    public void EnumerateWindows(Func<IntPtr, bool> callback)
    {
        EnumWindows((hWnd, _) => callback(hWnd), IntPtr.Zero);
    }
}