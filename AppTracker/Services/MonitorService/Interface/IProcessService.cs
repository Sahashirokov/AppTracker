using System;
using System.Diagnostics;
using System.Windows.Media;
using LauncherApp.MVVM.Model;

namespace LauncherApp.Services.MonitorService.Interface;

public interface IProcessService
{
    bool TryGetProcessInfo(IntPtr hWnd, out ApplicationInfo info);
    bool IsSystemProcess(Process process);
    ImageSource GetProcessIcon(Process process);
    string GetProcessPathSafe(Process process);
}