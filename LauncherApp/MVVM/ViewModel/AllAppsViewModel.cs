using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using LauncherApp.Command;
using LauncherApp.Messanger;
using LauncherApp.MVVM.Model;
using LauncherApp.Services;
using Microsoft.Win32;

namespace LauncherApp.MVVM.ViewModel;

public class AllAppsViewModel : BaseVm
{
    private readonly IApplicationMonitorService _monitorService;
    private readonly IFavoriteAppService _favoriteService;
    private readonly INotificationService _notification;
    private readonly IMessenger _messenger;
    
    private ObservableCollection<ApplicationInfoWrapper> _allApplications = new();
    private ObservableCollection<ApplicationInfoWrapper> _filteredApplications = new();
    private HashSet<string> _favoriteKeys = new();
    private DispatcherTimer _timer;
    private string _searchApp;
    public string SearchApp
    {
        get => _searchApp;
        set
        {
            if (SetField(ref _searchApp, value))
            {
               FilterApplications();
            }
        }
    }

    public ObservableCollection<ApplicationInfoWrapper> Applications
    {
        get => _filteredApplications;
        set
        {
            if (SetField(ref _filteredApplications, value))
            {
                OnPropertyChanged();
            }
        }
    }

    public DelegateCommand<ApplicationInfoWrapper> AddToFavoriteCommand { get; }
    public DelegateCommand ManualAddToFavoriteCommand { get; }

    public AllAppsViewModel(IApplicationMonitorService monitor, 
                           IFavoriteAppService favorite,
                           INotificationService notification,
                           IMessenger messenger)
    {
        _monitorService = monitor;
        _favoriteService = favorite;
        _notification = notification;
        _messenger = messenger;

        _monitorService.ApplicationsChanged += UpdateApps;
        
        AddToFavoriteCommand = new DelegateCommand<ApplicationInfoWrapper>(AddToFavorite, 
            app => !_favoriteKeys.Contains($"{app.Info.Name}|{app.Info.Path}"));
        ManualAddToFavoriteCommand = new DelegateCommand(AddManualFavorite);
        InitializeAsync();
        StartTimer();
    }

    private async void InitializeAsync()
    {
        await LoadFavorites();
        UpdateApps();
    }

    private void StartTimer()
    {
        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _timer.Tick += Timer_Tick;
        _timer.Start();
    }
    private void Timer_Tick(object sender, EventArgs e)
    {
        foreach (var wrapper in Applications)
        {
            wrapper.Info.RefreshDuration();
        }
    }
    private async Task LoadFavorites()
    {
        var favorites = await _favoriteService.LoadAppsAsync();
        _favoriteKeys = new HashSet<string>(favorites.Select(f => $"{f.Path}"));
        Applications.ToList().ForEach(a => a.IsFavorite = _favoriteKeys.Contains($"{a.Info.Path}"));
    }
  private void UpdateApps(object sender = null, EventArgs e = null)
{
    var installedApps = _monitorService.GetAllInstalledApplications().ToList();
    var newApps = _monitorService.GetAllWindowedApplications().ToList();

    Application.Current.Dispatcher.Invoke(() =>
    {
        var currentApps = _allApplications.ToDictionary(a => a.Info.Path);
        
        foreach (var installedApp in installedApps)
        {
            var runningApp = newApps.FirstOrDefault(r => 
                r.Path.Equals(installedApp.Path, StringComparison.OrdinalIgnoreCase));

            if (currentApps.TryGetValue(installedApp.Path, out var existingWrapper))
            {
                existingWrapper.Info.IsRunning = runningApp != null;
                if (runningApp != null)
                {
                    existingWrapper.Info.StartTime = runningApp.StartTime;
                }
            }
            else
            {
                _allApplications.Add(new ApplicationInfoWrapper(
                    new ApplicationInfo 
                    {
                        Name = installedApp.Name,
                        WindowTitle = installedApp.Name,
                        Path = installedApp.Path,
                        Icon = installedApp.Icon,
                        IsRunning = runningApp != null,
                        StartTime = runningApp?.StartTime ?? DateTime.MinValue
                    }, 
                    _favoriteKeys.Contains(installedApp.Path))
                );
            }
        }

        // Обработка работающих приложений
        foreach (var app in newApps)
        {
            if (currentApps.TryGetValue(app.Path, out var existing))
            {
                existing.Info.WindowTitle = app.WindowTitle;
                existing.Info.StartTime = app.StartTime;
            }
            else
            {
                var isInstalled = installedApps.Any(i => 
                    i.Path.Equals(app.Path, StringComparison.OrdinalIgnoreCase));
                if (!isInstalled && !_allApplications.Any(a => 
                        a.Info.Path.Equals(app.Path, StringComparison.OrdinalIgnoreCase)))
                {
                    _allApplications.Add(new ApplicationInfoWrapper(app, 
                        _favoriteKeys.Contains(app.Path)));
                }
            }
        }

        // Удаление отсутствующих
        var toRemove = _allApplications
            .Where(a => 
                !installedApps.Any(i => i.Path.Equals(a.Info.Path, StringComparison.OrdinalIgnoreCase)) && 
                !newApps.Any(r => r.Path.Equals(a.Info.Path, StringComparison.OrdinalIgnoreCase)))
            .ToList();
        foreach (var item in toRemove)
        {
            _allApplications.Remove(item);
        }

        FilterApplications(); // Обновляем фильтр после всех изменений
    });
}
    private void FilterApplications()
    {
        var filtered = string.IsNullOrEmpty(_searchApp)
            ? _allApplications
            : _allApplications.Where(a => 
                a.Info.Name.IndexOf(_searchApp, StringComparison.OrdinalIgnoreCase) >= 0);
        Applications.Clear();
        foreach (var app in filtered.OrderByDescending(a => a.Info.IsRunning))
        {
            Applications.Add(app);
        }
    }
    private async void AddToFavorite(ApplicationInfoWrapper wrapper)
    {
        try
        {
            var app = wrapper.Info;
            await _favoriteService.AddAsync(new AppM {
                Name = app.Name,
                Path = app.Path,
                WindowTitle = app.WindowTitle,
                Icon = Icon.ExtractAssociatedIcon(app.Path)?.ToString(),
                StartTime = app.StartTime,
                Version = "1.0.0",
            });
            
            _favoriteKeys.Add($"{app.Path}");
            wrapper.IsFavorite = true;
            _notification.ShowSuccess($"{app.Name} added to favorites!");
            _messenger.Send(new RefreshFavoritesMessage());
        }
        catch (Exception ex)
        {
            _notification.ShowError($"Error: {ex.Message}");
        }
    }

    private async void AddManualFavorite()
    {
        try
        {
            var dialog = new OpenFileDialog { Filter = "Executable files (*.exe)|*.exe" };
            if (dialog.ShowDialog() != true) return;

            var exePath = dialog.FileName;
            var version = FileVersionInfo.GetVersionInfo(exePath);
            
            await _favoriteService.AddAsync(new AppM {
                Name = version.FileDescription ?? Path.GetFileNameWithoutExtension(exePath),
                Path = exePath,
                WindowTitle = version.FileDescription ?? Path.GetFileNameWithoutExtension(exePath),
                Icon = Icon.ExtractAssociatedIcon(exePath)?.ToString(),
                StartTime = DateTime.Now,
                Version = version.FileVersion ?? "1.0.0"
            });
            
            await LoadFavorites();
            _notification.ShowSuccess("Application added manually!");
            _messenger.Send(new RefreshFavoritesMessage());
        }
        catch (Exception ex)
        {
            _notification.ShowError($"Manual add error: {ex.Message}");
        }
    }
    public void Dispose()
    {
        _timer.Tick -= Timer_Tick; // Important for proper cleanup
        _timer.Stop();
    }
}