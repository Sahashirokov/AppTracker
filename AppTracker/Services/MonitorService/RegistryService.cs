using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LauncherApp.MVVM.Model;
using LauncherApp.Services.MonitorService.Interface;
using Microsoft.Win32;

namespace LauncherApp.Services;

public class RegistryService: IRegistryService
{
    public IEnumerable<ApplicationInfo> GetInstalledApplications()
    {
        var apps = new List<ApplicationInfo>();
        var registryPaths = new[]
        {
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
            @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
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
                    exePath = exePath.Split(',')[0].Trim('"');
                    if (!File.Exists(exePath)) continue;
                    
                    apps.Add(new ApplicationInfo
                    {
                        Name = displayName,
                        Path = exePath,
                        Publisher = publisher,
                        InstallLocation = installLocation,
                        IsRunning = false,
                        Icon = IconExtractor.GetIcon(exePath),
                    });
                }
            }
        }

        return apps.DistinctBy(a => a.Path)
            .Where(s => s.Publisher != null && 
                        !s.Publisher.Contains("Microsoft Corporation", StringComparison.OrdinalIgnoreCase));
    }
}