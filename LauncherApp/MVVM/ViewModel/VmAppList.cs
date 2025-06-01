using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
    public ICommand ToggleAppCommand { get; }
    private Dictionary<string, Process> _processes = new();
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
        ToggleAppCommand = new DelegateCommand<ApplicationInfo>(ToggleApplication);
        _ = LoadApps();
        _messenger.Register<RefreshFavoritesMessage>(this, OnRefreshRequested);
        StartTimer();
        _monitorService.ApplicationsChanged += UpdateApp;
    }
    private void ToggleApplication(ApplicationInfo app)
    {
        if (app == null || string.IsNullOrEmpty(app.Path)) return;

        if (_processes.TryGetValue(app.Path, out var process) && !process.HasExited)
        {
            StopApplication(app, process);
        }
        else
        {
            StartApplication(app);
        }
    }
    private void StartApplication(ApplicationInfo app)
    {
        if (string.IsNullOrEmpty(app.Path) || !File.Exists(app.Path))
        {
            MessageBox.Show("Путь к приложению недействителен.");
            return;
        }

        try
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = app.Path,
                UseShellExecute = true
            };

            var process = Process.Start(startInfo);
            _processes[app.Path] = process;

            process.EnableRaisingEvents = true;
            process.Exited += (s, e) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    app.IsRunning = false;
                    _processes.Remove(app.Path);
                });
            };

            app.IsRunning = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Не удалось запустить приложение: {ex.Message}");
        }
    }
    private async void StopApplication(ApplicationInfo app, Process process)
    {
        try
        {
            if (!process.HasExited)
            {
                process.Kill();
                process.WaitForExit();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка остановки: {ex.Message}");
        }
        finally
        {
            await UpdateTotalTimeAsync(app);
            process.Dispose();
            _processes.Remove(app.Path);
            app.IsRunning = false;
        }
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
    
    private async void UpdateApp(object sender = null, EventArgs? e = null)
    {
        try
        {
            // Получаем все работающие процессы
            var runningApps = _monitorService.GetAllRunningApplications()
                .ToDictionary(a => a.Path, StringComparer.OrdinalIgnoreCase);

            // Обновляем состояние для каждого приложения
            foreach (var item in AppM)
            {
                if (runningApps.TryGetValue(item.Path, out var runningApp))
                {
                    item.IsRunning = true;
                    item.StartTime = runningApp.StartTime;

                    // Если процесс ещё не отслеживается
                    if (!_processes.ContainsKey(item.Path))
                    {
                        var process = Process.GetProcessById(runningApp.ProcessId);
                        _processes[item.Path] = process;

                        // Подписываемся на завершение процесса
                        process.EnableRaisingEvents = true;
                        process.Exited += async (s, ev) =>
                        {
                            await Application.Current.Dispatcher.InvokeAsync(async () =>
                            {
                                await UpdateTotalTimeAsync(item); // Теперь await работает
                                item.IsRunning = false;
                                _processes.Remove(item.Path);
                            });
                        };
                    }
                }
                else
                {
                    item.IsRunning = false;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка обновления состояния: {ex.Message}");
        }
    }
    private async Task UpdateTotalTimeAsync(ApplicationInfo app)
    {
        if (app.StartTime == default) return;

        var duration = DateTime.Now - app.StartTime;
        app.TotalTime += duration;

        try
        {
            var dbApp = await _favoriteAppService.GetAppByIdAsync(app.id);
            dbApp.TotalTime = app.TotalTime;
            await _favoriteAppService.UpdateAsync(dbApp);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка сохранения времени: {ex.Message}");
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
                     TotalTime = item.TotalTime,
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