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
    private ObservableCollection<ApplicationInfoWrapper> _applications = new(); 

    public ObservableCollection<ApplicationInfoWrapper> Applications 
    {
        get => _applications;
        set => SetField(ref _applications, value);
    }

    private readonly IFavoriteAppService _favoriteAppService;
    private readonly INotificationService _notificationService;
    private HashSet<string> _existingFavorites = new();
    public DelegateCommand<ApplicationInfoWrapper> AddToFavoriteCommand { get; }
    private readonly IMessenger _messenger;
    public AllAppsViewModel(IApplicationMonitorService monitorService,IFavoriteAppService favoriteAppService,INotificationService notificationService,IMessenger messenger)
    {
        _monitorService = monitorService;
        _monitorService.ApplicationsChanged += OnApplicationsChanged;
        _favoriteAppService = favoriteAppService;
        _notificationService = notificationService;
        _messenger = messenger;
        InitializeAsync();
        AddToFavoriteCommand = new DelegateCommand<ApplicationInfoWrapper>(AddToFavorite,CanAddToFavorite);
    }
    private async Task InitializeAsync()
    {
        await LoadExistingFavorites(); // Загрузка избранного при старте
        LoadApplications();           // Загрузка приложений
    }
    private bool CanAddToFavorite(ApplicationInfoWrapper appInfo)
    {
        var key = $"{appInfo.Info
            .Name}|{appInfo.Info
            .Path}";
        return !_existingFavorites.Contains(key);
    }
    
    private async void AddToFavorite(ApplicationInfoWrapper wrapper)
    {
        try
        {
            Console.WriteLine($"Adding new favorite app: {wrapper.Info.Name}");
            var appInfo = wrapper.Info;
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
            wrapper.IsFavorite = true;
            
            CommandManager.InvalidateRequerySuggested();
            _notificationService.ShowSuccess($"{appInfo.Name} добавлено в избранное!");
            
            _messenger.Send(new RefreshFavoritesMessage());
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка: {ex.Message}");
        }
    }
    // public ObservableCollection<ApplicationInfo> Applications
    // {
    //     get => _applications;
    //     set
    //     {
    //         _applications = value;
    //         foreach (var application in _applications)
    //         {
    //             Console.WriteLine(application.Name);
    //         }
    //         OnPropertyChanged();
    //     }
    // }
    private async void LoadApplications()
    {
        try
        {
            var apps = await Task.Run(() => 
                _monitorService.GetVisibleApplications().ToList());
           Application.Current.Dispatcher.Invoke(() => 
            {
                Applications.Clear();
                foreach (var app in apps)
                {
                    var key = $"{app.Name}|{app.Path}";
                    var wrapper = new ApplicationInfoWrapper(app, _existingFavorites.Contains(key));
                    Applications.Add(wrapper);
                }
                
            });
            await LoadExistingFavorites();
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки приложений: {ex.Message}");
        }
    }
    private async Task LoadExistingFavorites()
    {
        try
        {
            var favorites = await _favoriteAppService.LoadAppsAsync();
            _existingFavorites = new HashSet<string>(
                favorites.Select(f => $"{f.Name}|{f.Path}"));
        
            // Обновляем все существующие обёртки
            foreach(var wrapper in Applications)
            {
                wrapper.IsFavorite = _existingFavorites.Contains(
                    $"{wrapper.Info.Name}|{wrapper.Info.Path}");
            }
        }
        catch(Exception ex)
        {
            _notificationService.ShowError($"Ошибка загрузки избранного: {ex.Message}");
        }
    }
    private void OnApplicationsChanged(object sender, EventArgs e) => LoadApplications();
}