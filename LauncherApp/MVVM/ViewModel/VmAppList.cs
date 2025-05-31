using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using LauncherApp.Command;
using LauncherApp.Messanger;
using LauncherApp.Model;
using LauncherApp.MVVM.Model;
using LauncherApp.Services;
using MediatR;

namespace LauncherApp.ViewModel;

public class VmAppList:BaseVm
{
    private readonly IFavoriteAppService _favoriteAppService;
     public ObservableCollection<ApplicationInfo> AppM { get; set; }
    private bool _isLoading;
    private readonly IMessenger _messenger;
    public DelegateCommand<ApplicationInfo> RemoveApp { get; }
    private readonly IApplicationMonitorService _monitorService;
    private DispatcherTimer _timer;
    public VmAppList(IFavoriteAppService favoriteAppService,IMessenger messenger,IApplicationMonitorService monitor)
    {
        _favoriteAppService = favoriteAppService;
        AppM = new ObservableCollection<ApplicationInfo>();
        _messenger = messenger;
        _monitorService = monitor;
        LoadAppsCommand = new DelegateCommand(async () => await LoadApps());
        RemoveApp = new DelegateCommand<ApplicationInfo>(async (item)=> await DeleteApp(item),(item)=>item != null);
        _ = LoadApps();
        _messenger.Register<RefreshFavoritesMessage>(this, OnRefreshRequested);
        StartTimer();
        _monitorService.ApplicationsChanged += UpdateApp;
    }
    

    private void StartTimer()
    {
        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _timer.Tick += Timer_Tick;
        _timer.Start();
    }
    private void Timer_Tick(object sender, EventArgs e)
    {
        foreach (var app in AppM)
        {
            app.RefreshDuration();
        }
    }
    public DelegateCommand LoadAppsCommand { get; }
   
    public bool IsLoading
    {
        get => _isLoading;
        set => SetField(ref _isLoading, value);
    }
    private void OnRefreshRequested(RefreshFavoritesMessage message)
    {
        LoadApps();
    }
    
    private void UpdateApp(object sender = null, EventArgs? e = null)
    {
        try
        {
            var activeApps = _monitorService.GetFilteredVisibleApplications(AppM).ToList();
            var runningByPath = activeApps.ToDictionary(a => a.Path, a => a.StartTime);

            foreach (var item in AppM)
            {
                if (runningByPath.TryGetValue(item.Path, out var startTime))
                {
                    item.IsRunning = true;
                    item.StartTime = startTime;
                }
                else
                {
                    item.IsRunning = false;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }
    //TODO 1) обновлять путь из метода GetAllInstalledApplications() при загрузке
    //TODO 2) сделать запуск и остановку приложения
    //TODO 2.1) Менять цвет кнопки
    //TODO 3) Сделать запись в БД общее время(TOTAL TIME)
   
    private async Task LoadApps()
    {
        try
        {
            IsLoading = true;
           // await Task.Delay(3500);
            var result =  await _favoriteAppService.LoadAppsAsync();
            
            Console.WriteLine(result.Count);
             AppM.Clear();
             foreach (var item in result)
             {
                 Console.WriteLine(item.Path);
                 AppM.Add(new ApplicationInfo()
                 {
                     id = item.Id,
                     Name = item.Name,
                     IsRunning = false,
                     Icon = IconExtractor.GetIcon(item.Path.Split(',')[0].Trim('"')),
                     Path = item.Path,
                     WindowTitle = item.WindowTitle,
                     StartTime = item.StartTime,
                 });
             }
           
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            throw;
        }
        finally
        {
            IsLoading = false;
        }
    }
    private async Task DeleteApp(ApplicationInfo app)
    {
        await _favoriteAppService.DeleteAsync(app.id);
        await LoadApps();
    }
    public void Dispose()
    {
        _timer.Tick -= Timer_Tick; 
        _timer.Stop();
    }
}