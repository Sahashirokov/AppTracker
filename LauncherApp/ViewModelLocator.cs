using System;
using System.IO;
using System.Reflection;
using LauncherApp.Data;
using LauncherApp.Messanger;
using LauncherApp.MVVM.ViewModel;
using LauncherApp.MVVM.View.Pages;
using LauncherApp.Repository.FavoriteAppRepository;
using LauncherApp.Services;
using LauncherApp.Services.ManagerWindow;
using LauncherApp.Services.MonitorService;
using LauncherApp.Services.MonitorService.Interface;
using LauncherApp.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AllAppsPage = LauncherApp.MVVM.View.Pages.AllAppsPage;
using FavoritePage = LauncherApp.MVVM.View.Pages.FavoritePage;

namespace LauncherApp;

public class ViewModelLocator
{
    private static IServiceProvider _serviceProvider;
    
    public static void Init()
    {
        var services = new ServiceCollection();
        
        services.AddDbContext<AppDbContext>(options => 
            { var path = Path.Combine(
                    AppDomain.CurrentDomain.BaseDirectory, 
                    "AppTrackerDB.db"
                );
                options.UseSqlite($"Data Source={path}");},
            ServiceLifetime.Singleton);
        
        services.AddSingleton<IWindowService, WindowService>();
        services.AddSingleton<IStartupService, StartupService>();
        
        services.AddSingleton<PageServices>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddScoped<IFavoriteAppRepository, AppFavoriteRepository>();
        services.AddTransient<IFavoriteAppService, FavoriteAppService>();
        //
        services.AddSingleton<IWindowEnumerator, WindowEnumerator>();
        services.AddSingleton<IProcessService, ProcessService>();
        services.AddSingleton<IRegistryService, RegistryService>();
        services.AddSingleton<IApplicationCollector, ApplicationCollector>();
        services.AddSingleton<IApplicationMonitorService, ApplicationMonitorService>();
        //
        services.AddSingleton<AppStateService>();
        
        services.AddTransient<MainViewModel>();
        services.AddTransient<HeaderViewModel>();
        services.AddTransient<FavoritePage>();
        services.AddTransient<AllAppsPage>();
        services.AddTransient<INotificationService, MessageBoxNotificationService>();
        services.AddSingleton<IMessenger, Messenger>();
        services.AddSingleton<VmAppList>();
        services.AddSingleton<AllAppsViewModel>();
        _serviceProvider = services.BuildServiceProvider();
        
        var context = _serviceProvider.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated();
        var dbPath = context.Database.GetDbConnection().DataSource;
        Console.WriteLine($"Database path: {dbPath}");
        foreach (var item in services)
        {
            _serviceProvider.GetRequiredService(item.ServiceType);
        }
    }
    public MainViewModel MainViewModel => _serviceProvider.GetRequiredService<MainViewModel>();
    public VmAppList VmAppList => _serviceProvider.GetRequiredService<VmAppList>();
    public AllAppsViewModel AllAppsViewModel => _serviceProvider.GetRequiredService<AllAppsViewModel>();
    public HeaderViewModel HeaderViewModel => _serviceProvider.GetRequiredService<HeaderViewModel>();
}