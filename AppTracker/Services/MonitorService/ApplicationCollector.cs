using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using LauncherApp.MVVM.Model;
using LauncherApp.Services.MonitorService.Interface;

namespace LauncherApp.Services.MonitorService;

public class ApplicationCollector : IApplicationCollector
{
    private readonly IWindowEnumerator _windowEnumerator;
    private readonly IProcessService _processService;
    private readonly IntPtr _shellWindow;

    public ApplicationCollector(IWindowEnumerator windowEnumerator, IProcessService processService)
    {
        _windowEnumerator = windowEnumerator;
        _processService = processService;
        _shellWindow = GetShellWindow();
    }

    [DllImport("user32.dll")]
    private static extern IntPtr GetShellWindow();

    public IEnumerable<ApplicationInfo> GetVisibleApplications()
    {
        var windows = new List<ApplicationInfo>();
        _windowEnumerator.EnumerateWindows(hWnd =>
        {
            if (!IsWindowValid(hWnd)) return true;
            
            if (_processService.TryGetProcessInfo(hWnd, out var info))
            {
                windows.Add(info);
            }
            return true;
        });

        return windows.DistinctBy(a => a.ProcessId);
    }

    public IEnumerable<ApplicationInfo> GetFilteredVisibleApplications(ObservableCollection<ApplicationInfo> filter)
    {
        var filtered = GetVisibleApplications()
            .Where(app => filter.Any(f => f.Name == app.Name || f.Path ==app.Path))
            .ToList();
        
        return filtered;
    }

    private bool IsWindowValid(IntPtr hWnd)
    {
        if (hWnd == _shellWindow || !IsWindowVisible(hWnd)) return false;
        
        var style = GetWindowLong(hWnd, GWL_STYLE);
        return (style & WS_VISIBLE) != 0 && !string.IsNullOrEmpty(GetWindowTitle(hWnd));
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

    [DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);
    
    private const int GWL_STYLE = -16;
    private const int WS_VISIBLE = 0x10000000;
}