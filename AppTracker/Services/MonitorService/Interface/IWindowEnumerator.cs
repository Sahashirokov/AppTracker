using System;

namespace LauncherApp.Services.MonitorService.Interface;

public interface IWindowEnumerator
{
    void EnumerateWindows(Func<IntPtr, bool> callback);
}