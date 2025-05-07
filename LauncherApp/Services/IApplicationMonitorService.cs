using System;
using System.Collections.Generic;
using LauncherApp.MVVM.Model;

namespace LauncherApp.Services;

public interface IApplicationMonitorService
{
    IEnumerable<ApplicationInfo> GetVisibleApplications();
    event EventHandler ApplicationsChanged;
}