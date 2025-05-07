using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using LauncherApp.MVVM.Model;
using LauncherApp.Services;

namespace LauncherApp.MVVM.ViewModel;

public class AllAppsViewModel: BaseVm
{
    private readonly IApplicationMonitorService _monitorService;
    private ObservableCollection<ApplicationInfo> _applications = new();


    public AllAppsViewModel(IApplicationMonitorService monitorService)
    {
        _monitorService = monitorService;
        _monitorService.ApplicationsChanged += OnApplicationsChanged;
        LoadApplications();
    }
    public ObservableCollection<ApplicationInfo> Applications
    {
        get => _applications;
        set
        {
            _applications = value;
            foreach (var application in _applications)
            {
                Console.WriteLine(application.Name);
            }
            OnPropertyChanged();
        }
    }
    private async void LoadApplications()
    {
        try
        {
            var apps = await Task.Run(() => 
                _monitorService.GetVisibleApplications().ToList());
        
            // Используем правильный способ доступа к Dispatcher
           Application.Current.Dispatcher.Invoke(() => 
            {
                Applications.Clear();
                foreach (var app in apps)
                {
                    Applications.Add(app);
                }
            });
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки приложений: {ex.Message}");
        }
    }

    private void OnApplicationsChanged(object sender, EventArgs e) => LoadApplications();
}