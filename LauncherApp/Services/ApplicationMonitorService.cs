
using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LauncherApp.MVVM.Model;
using LauncherApp.Services.MonitorService.Interface;
using Microsoft.Win32;

namespace LauncherApp.Services;

public class ApplicationMonitorService: IApplicationMonitorService
{
    private readonly Timer _refreshTimer;
    private readonly IApplicationCollector _applicationCollector;
    private readonly IRegistryService _registryService;
    private readonly IProcessService _processService;
    public event EventHandler? ApplicationsChanged;

    public ApplicationMonitorService(
        IApplicationCollector applicationCollector,
        IRegistryService registryService,IProcessService processService)
    {
        _applicationCollector = applicationCollector;
        _registryService = registryService;
        _processService = processService;
        _refreshTimer = new Timer(5000);
        _refreshTimer.Elapsed += (s, e) => ApplicationsChanged?.Invoke(this, EventArgs.Empty);
        _refreshTimer.AutoReset = true;
        _refreshTimer.Start();
        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
        {
            ApplicationsChanged?.Invoke(this, EventArgs.Empty);
        }));
    }

    public IEnumerable<ApplicationInfo> GetVisibleApplications() => 
        _applicationCollector.GetVisibleApplications();
    
    public IEnumerable<ApplicationInfo> GetFilteredVisibleApplications(ObservableCollection<ApplicationInfo> filter) => 
        _applicationCollector.GetFilteredVisibleApplications(filter);

    public IEnumerable<ApplicationInfo> GetAllInstalledApplications() => 
        _registryService.GetInstalledApplications();

    public IEnumerable<ApplicationInfo> GetAllWindowedApplications() => 
        Process.GetProcesses()
            .Where(p => !string.IsNullOrEmpty(p.MainWindowTitle) && p.MainWindowHandle != IntPtr.Zero)
            .Select(p => CreateApplicationInfo(p))
            .Where(app => app != null)
            .DistinctBy(p => p.Path);

    private ApplicationInfo CreateApplicationInfo(Process p)
    {
        try
        {
            return new ApplicationInfo
            {
                ProcessId = p.Id,
                Name = p.ProcessName,
                Path = _processService.GetProcessPathSafe(p),
                WindowTitle = p.MainWindowTitle,
                StartTime = p.StartTime,
                Icon = _processService.GetProcessIcon(p),
                IsRunning = true,
            };
        }
        catch
        {
            return null;
        }
    }
}