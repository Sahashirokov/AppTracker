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
    
    private ObservableCollection<ApplicationInfoWrapper> _apps = new();
    private HashSet<string> _favoriteKeys = new();
    private DispatcherTimer _timer;

    public ObservableCollection<ApplicationInfoWrapper> Applications
    {
        get => _apps;
        set => SetField(ref _apps, value);
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
        _timer.Tick += (s, e) => Applications.ToList().ForEach(a => a.Info.RefreshDuration());
        _timer.Start();
    }
    private async Task LoadFavorites()
    {
        var favorites = await _favoriteService.LoadAppsAsync();
        _favoriteKeys = new HashSet<string>(favorites.Select(f => $"{f.Name}|{f.Path}"));
        Applications.ToList().ForEach(a => a.IsFavorite = _favoriteKeys.Contains($"{a.Info.Name}|{a.Info.Path}"));
    }
    private void UpdateApps(object sender = null, EventArgs e = null)
    {
        var newApps = _monitorService.GetVisibleApplications().ToList();
        
        Application.Current.Dispatcher.Invoke(() =>
        {
            // Обновление существующих
            var current = Applications.ToDictionary(a => a.Info.ProcessId);
            foreach (var app in newApps)
            {
                if (current.TryGetValue(app.ProcessId, out var existing))
                {
                    existing.Info.WindowTitle = app.WindowTitle;
                    existing.Info.StartTime = app.StartTime;
                }
                else
                {
                    Applications.Add(new ApplicationInfoWrapper(app, 
                        _favoriteKeys.Contains($"{app.Name}|{app.Path}")));
                }
            }
            // Удаление отсутствующих
            var toRemove = Applications.Where(a => !newApps.Any(n => n.ProcessId == a.Info.ProcessId)).ToList();
            toRemove.ForEach(a => Applications.Remove(a));
        });
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
                StartTime = DateTime.Now,
                Version = "1.0.0"
            });
            
            _favoriteKeys.Add($"{app.Name}|{app.Path}");
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
}