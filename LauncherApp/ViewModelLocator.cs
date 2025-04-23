using System;
using LauncherApp.MVVM.ViewModel;
using LauncherApp.Pages;
using LauncherApp.Services;
using LauncherApp.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace LauncherApp;

public class ViewModelLocator
{
    private static IServiceProvider _serviceProvider;
    
    public static void Init()
    {
        var services = new ServiceCollection();
        
        services.AddSingleton<IWindowService, WindowService>();
        services.AddSingleton<PageServices>();
        services.AddSingleton<INavigationService, NavigationService>();
       
        
        services.AddTransient<MainViewModel>();
        services.AddTransient<AllAppsPage>();
        services.AddTransient<FavoritePage>();
        
        services.AddSingleton<VmAppList>();
        services.AddSingleton<AllAppsViewModel>();
        _serviceProvider = services.BuildServiceProvider();
        foreach (var item in services)
        {
            _serviceProvider.GetRequiredService(item.ServiceType);
        }
    }
    public MainViewModel MainViewModel => _serviceProvider.GetRequiredService<MainViewModel>();
    public VmAppList VmAppList => _serviceProvider.GetRequiredService<VmAppList>();
    public AllAppsViewModel AllAppsViewModel => _serviceProvider.GetRequiredService<AllAppsViewModel>();
}