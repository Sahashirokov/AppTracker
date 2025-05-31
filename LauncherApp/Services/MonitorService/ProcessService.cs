using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Media;
using LauncherApp.MVVM.Model;
using LauncherApp.Services.MonitorService.Interface;

namespace LauncherApp.Services.MonitorService;

public class ProcessService : IProcessService
{
    [DllImport("user32.dll")]
    private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

    [DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
    
    private const int GWL_STYLE = -16;
    private const int WS_VISIBLE = 0x10000000;

    public bool TryGetProcessInfo(IntPtr hWnd, out ApplicationInfo info)
    {
        info = null;
        GetWindowThreadProcessId(hWnd, out int processId);
        
        try
        {
            using var process = Process.GetProcessById(processId);
            if (!IsWindowVisible(hWnd) || IsSystemProcess(process)) return false;

            info = new ApplicationInfo
            {
                ProcessId = processId,
                Name = process.ProcessName,
                WindowTitle = GetWindowTitle(hWnd),
                StartTime = process.StartTime,
                Icon = GetProcessIcon(process),
                Path = GetProcessPathSafe(process),
            };
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool IsSystemProcess(Process process)
    {
        try
        {
            return process.MainModule?.FileName?.StartsWith(Environment.SystemDirectory) == true;
        }
        catch
        {
            return true;
        }
    }

    public ImageSource GetProcessIcon(Process process)
    {
        try
        {
            var path = process.MainModule?.FileName;
            if (string.IsNullOrEmpty(path)) return IconExtractor.DefaultIcon();
            return IconExtractor.GetIcon(path);
        }
        catch
        {
            return IconExtractor.DefaultIcon();
        }
    }

    public string GetProcessPathSafe(Process process)
    {
        try
        {
            return process.MainModule?.FileName ?? string.Empty;
        }
        catch
        {
            return string.Empty;
        }
    }

    private string GetWindowTitle(IntPtr hWnd)
    {
        var titleBuilder = new StringBuilder(256);
        GetWindowText(hWnd, titleBuilder, titleBuilder.Capacity);
        return titleBuilder.ToString();
    }

    [DllImport("user32.dll")]
    private static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
}