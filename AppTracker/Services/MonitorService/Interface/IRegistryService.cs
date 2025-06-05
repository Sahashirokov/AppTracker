using System.Collections.Generic;
using LauncherApp.MVVM.Model;

namespace LauncherApp.Services.MonitorService.Interface;

public interface IRegistryService
{
    IEnumerable<ApplicationInfo> GetInstalledApplications();
}