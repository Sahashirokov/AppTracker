
using System.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LauncherApp.MVVM.Model;
using Microsoft.Win32;

namespace LauncherApp.Services;

public class ApplicationMonitorService: IApplicationMonitorService
{
    private readonly Timer _refreshTimer;
    private IntPtr _shellWindow;
    public event EventHandler? ApplicationsChanged;

    public ApplicationMonitorService()
    {
        _shellWindow = GetShellWindow();
        
        //Тут мы запрашиваем каждые 5 секунд список активных приложений
        _refreshTimer = new Timer(5000); 
        
        _refreshTimer.Elapsed += OnTimerElapsed; 
        _refreshTimer.AutoReset = true;
        _refreshTimer.Start();
    }

    private void OnTimerElapsed(object? sender, System.Timers.ElapsedEventArgs e)
    {
        ApplicationsChanged?.Invoke(this, EventArgs.Empty);
    }

    public IEnumerable<ApplicationInfo> GetVisibleApplications()
    {
        var windows = new List<ApplicationInfo>();
        EnumWindowsProc callback = (hWnd, _) =>
        {
            if (!IsWindowVisible(hWnd)) return true;
            if (hWnd == _shellWindow) return true;
    
            var style = GetWindowLong(hWnd, GWL_STYLE);
            if ((style & WS_VISIBLE) == 0) return true;
    
            var title = GetWindowTitle(hWnd);
            if (string.IsNullOrEmpty(title)) return true;
                //Console.WriteLine($"Обработка окна: {hWnd}, Title: '{title}'");
            if (GetProcessInfo(hWnd, out var processInfo))
            {
                windows.Add(processInfo);
            }
    
            return true;
        };
    
        EnumWindows(callback, IntPtr.Zero);
        return windows.DistinctBy(a => a.ProcessId);
    }
    public IEnumerable<ApplicationInfo> GetAllInstalledApplications()
    {
        var apps = new List<ApplicationInfo>();
        var registryPaths = new[]
        {
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
            @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall" // Для 32-битных на 64-битной ОС
        };

        foreach (var path in registryPaths)
        {
            using var baseKey = Registry.LocalMachine.OpenSubKey(path);
            if (baseKey == null) continue;

            foreach (var subKeyName in baseKey.GetSubKeyNames())
            {
                using var subKey = baseKey.OpenSubKey(subKeyName);
                var displayName = subKey?.GetValue("DisplayName")?.ToString();
                var publisher = subKey?.GetValue("Publisher")?.ToString();
                var installLocation = subKey?.GetValue("InstallLocation")?.ToString();
                var exePath = subKey?.GetValue("DisplayIcon")?.ToString();

                if (!string.IsNullOrEmpty(displayName) && !string.IsNullOrEmpty(exePath))
                {
                    apps.Add(new ApplicationInfo
                    {
                        Name = displayName,
                        Path = exePath,
                        Publisher = publisher,
                        InstallLocation = installLocation,
                        IsRunning = false // Определится позже
                    });
                }
            }
        }

        return apps.DistinctBy(a => a.Path); // Убираем дубликаты
    }
    public IEnumerable<ApplicationInfo> GetAllWindowedApplications()
    {
        return Process.GetProcesses()
            .Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowHandle != IntPtr.Zero)
            .Select(p => new ApplicationInfo
            {
                ProcessId = p.Id,
                Name = p.ProcessName,
                Path = p.MainModule?.FileName,
                WindowTitle = p.MainWindowTitle,
                StartTime = p.StartTime,
                Icon =  GetProcessIcon(p),
                IsRunning = true,
            })
            .DistinctBy(p => p.Path); // Убедитесь, что нет дубликатов по пути
    }
    private bool GetProcessInfo(IntPtr hWnd, out ApplicationInfo info)
    {
        info = null;
        GetWindowThreadProcessId(hWnd, out int processId);
        // Console.WriteLine($"Process ID: {processId}");
        try
        {
            using var process = Process.GetProcessById(processId);
             // Console.WriteLine($"Process Name: {process.ProcessName}, MainModule: {process.MainModule?.FileName}");
            if (IsSystemProcess(process)) return false;

            info = new ApplicationInfo
            {
                ProcessId = processId,
                Name = process.ProcessName,
                WindowTitle = GetWindowTitle(hWnd),
                StartTime = process.StartTime,
                Icon = GetProcessIcon(process),
                Path =process.MainModule?.FileName, 
            };
            return true;
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
            return false;
        }
    }

    private ImageSource GetProcessIcon(Process process)
    {
        try
        {
            var path = process.MainModule?.FileName;
            if (string.IsNullOrEmpty(path)) return IconExtractor.DefaultIcon();
        
            // Нормализация пути для файловой системы
            path = Path.GetFullPath(path);
            return IconExtractor.GetIcon(path);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка получения иконки: {ex.Message}");
            return IconExtractor.DefaultIcon();
        }
    }

    private bool IsSystemProcess(Process process)
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
    private string GetWindowTitle(IntPtr hWnd)
    {
        var titleBuilder = new StringBuilder(256);
        GetWindowText(hWnd, titleBuilder, titleBuilder.Capacity);
        return titleBuilder.ToString();
    }
    // P/Invoke методы (аналогичны оригинальным, но без static)
    [DllImport("user32.dll")]
    private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

    [DllImport("user32.dll")]
    private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

    [DllImport("user32.dll")]
    private static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

    [DllImport("user32.dll")]
    private static extern bool IsWindowVisible(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern IntPtr GetShellWindow();

    private const int GWL_STYLE = -16;
    private const int WS_VISIBLE = 0x10000000;

    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
}