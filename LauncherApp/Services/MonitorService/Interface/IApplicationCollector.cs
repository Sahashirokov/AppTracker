using System.Collections.Generic;
using System.Collections.ObjectModel;
using LauncherApp.MVVM.Model;

namespace LauncherApp.Services.MonitorService.Interface;

public interface IApplicationCollector
{
    IEnumerable<ApplicationInfo> GetVisibleApplications();
    IEnumerable<ApplicationInfo> GetFilteredVisibleApplications(ObservableCollection<ApplicationInfo> filter);
}