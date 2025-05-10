using System;
using System.Collections.Generic;
using LauncherApp.MVVM.Model;

namespace LauncherApp.Services;

public interface IApplicationMonitorService
{
    IEnumerable<ApplicationInfo> GetVisibleApplications();
    IEnumerable<ApplicationInfo> GetAllWindowedApplications();
    IEnumerable<ApplicationInfo> GetAllInstalledApplications();
    event EventHandler ApplicationsChanged;
}