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
    public VmAppList(IFavoriteAppService favoriteAppService,IMessenger messenger)
    {
        _favoriteAppService = favoriteAppService;
        AppM = new ObservableCollection<ApplicationInfo>();
        _messenger = messenger;
        LoadAppsCommand = new DelegateCommand(async () => await LoadApps());
        RemoveApp = new DelegateCommand<ApplicationInfo>(async (item)=> await DeleteApp(item),(item)=>item != null);
        _ = LoadApps();
        _messenger.Register<RefreshFavoritesMessage>(this, OnRefreshRequested);
    }
    public DelegateCommand LoadAppsCommand { get; }
    // public DelegateCommand IsRunning { get; }
    // public ICommand ToggleCommand => new DelegateCommand<AppChecker>(item => 
    // {
    //     item.IsRunning = !item.IsRunning;
    // });
    public bool IsLoading
    {
        get => _isLoading;
        set => SetField(ref _isLoading, value);
    }
    private void OnRefreshRequested(RefreshFavoritesMessage message)
    {
        LoadApps();
    }
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
        catch (Exception e)
        {
            MessageBox.Show(e.Message);
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
        LoadApps();
    }
}