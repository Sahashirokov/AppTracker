namespace LauncherApp.Services;

public class StartupService : IStartupService
{
    internal const string KeyPath = @"Software\Microsoft\Windows\CurrentVersion\Run";
    internal const string AppName = "AppTracker";

    public void EnableAutoStart()
    {
        using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(KeyPath, true))
        {
            key.SetValue(AppName, $"\"{System.Reflection.Assembly.GetExecutingAssembly().Location}\"");
        }
    }

    public void DisableAutoStart()
    {
        using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(KeyPath, true))
        {
            key.DeleteValue(AppName, false);
        }
    }
}