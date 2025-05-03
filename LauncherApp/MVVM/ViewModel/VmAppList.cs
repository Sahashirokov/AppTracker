using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using LauncherApp.Command;
using LauncherApp.Model;
using LauncherApp.MVVM.Model;
using LauncherApp.Services;
using MediatR;

namespace LauncherApp.ViewModel;

public class VmAppList:BaseVm
{
    private readonly IFavoriteAppService _favoriteAppService;
     public ObservableCollection<AppM> AppM { get; set; }
    private bool _isLoading;
    public VmAppList(IFavoriteAppService favoriteAppService)
    {
        _favoriteAppService = favoriteAppService;
        AppM = new ObservableCollection<AppM>();
        LoadAppsCommand = new DelegateCommand(async () => await LoadApps());
        _ = LoadApps();
    }
    public DelegateCommand LoadAppsCommand { get; }
    
    public bool IsLoading
    {
        get => _isLoading;
        set => SetField(ref _isLoading, value);
    }

    private async Task LoadApps()
    {
        try
        {
            IsLoading = true;
            await Task.Delay(3500);
            var result =  await _favoriteAppService.LoadAppsAsync();
            Console.WriteLine(result.Count);
             AppM.Clear();
             foreach (var item in result)
             {
                 AppM.Add(item);
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
}