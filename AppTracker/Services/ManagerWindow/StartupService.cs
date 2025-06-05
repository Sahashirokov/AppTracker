using System;

namespace LauncherApp.Services;

public class StartupService : IStartupService
{
    internal const string KeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
    internal const string AppName = "AppTracker";

    public void EnableAutoStart()
    {
        // Получаем путь к главному исполняемому файлу
        string exePath = GetExecutablePath();
        
        using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(KeyPath, true))
        {
            key.SetValue(AppName, $"\"{exePath}\"");
        }
    }

    public void DisableAutoStart()
    {
        using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(KeyPath, true))
        {
            key.DeleteValue(AppName, false);
        }
    }
    private string GetExecutablePath()
    {
        return AppContext.BaseDirectory + 
               System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe";
    }
}