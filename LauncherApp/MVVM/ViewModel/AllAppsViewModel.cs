using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LauncherApp.Command;
using LauncherApp.Messanger;
using LauncherApp.MVVM.Model;
using LauncherApp.Services;

namespace LauncherApp.MVVM.ViewModel;

public class AllAppsViewModel: BaseVm
{
    private readonly IApplicationMonitorService _monitorService;
    private ObservableCollection<ApplicationInfo> _applications = new();
    private readonly IFavoriteAppService _favoriteAppService;
    private readonly INotificationService _notificationService;
    private HashSet<string> _existingFavorites = new();
    public DelegateCommand<ApplicationInfo> AddToFavoriteCommand { get; }
    private readonly IMessenger _messenger;
    public AllAppsViewModel(IApplicationMonitorService monitorService,IFavoriteAppService favoriteAppService,INotificationService notificationService,IMessenger messenger)
    {
        _monitorService = monitorService;
        _monitorService.ApplicationsChanged += OnApplicationsChanged;
        _favoriteAppService = favoriteAppService;
        _notificationService = notificationService;
        _messenger = messenger;
        LoadApplications();
        AddToFavoriteCommand = new DelegateCommand<ApplicationInfo>(AddToFavorite,CanAddToFavorite);
    }
    private bool CanAddToFavorite(ApplicationInfo appInfo)
    {
        var key = $"{appInfo.Name}|{appInfo.Path}";
        return !_existingFavorites.Contains(key);
    }
    private async void AddToFavorite(ApplicationInfo appInfo)
    {
        try
        {
            var appM = new AppM
            {
                Name = appInfo.Name,
                WindowTitle = appInfo.WindowTitle,
                Path = appInfo.Path,
                Icon = appInfo.Icon.ToString(),
                StartTime = appInfo.StartTime,
                Version = "1.0.0",
                TotalTime = appInfo.Duration
            };
            
            await _favoriteAppService.AddAsync(appM);
            _existingFavorites.Add($"{appM.Name}|{appM.Path}");
            CommandManager.InvalidateRequerySuggested();
            _notificationService.ShowSuccess($"{appInfo.Name} добавлено в избранное!");
            
            _messenger.Send(new RefreshFavoritesMessage());
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка: {ex.Message}");
        }
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