using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LauncherApp.MVVM.Model;

namespace LauncherApp.Services;

public interface IApplicationMonitorService
{
    IEnumerable<ApplicationInfo> GetVisibleApplications();
    IEnumerable<ApplicationInfo> GetFilteredVisibleApplications(ObservableCollection<ApplicationInfo> applicationInfos);
    IEnumerable<ApplicationInfo> GetAllWindowedApplications();
    IEnumerable<ApplicationInfo> GetAllRunningApplications();
    IEnumerable<ApplicationInfo> GetAllInstalledApplications();
    event EventHandler ApplicationsChanged;
}