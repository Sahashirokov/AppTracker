using System;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace LauncherApp.Services;

public class NavigationService:INavigationService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly PageServices _pageServices;

    public NavigationService(IServiceProvider serviceProvider, PageServices pageServices)
    {
        _serviceProvider = serviceProvider;
        _pageServices = pageServices;
    }

    public void NavigateTo<T>() where T : Page => NavigateTo(typeof(T));
    
    public void NavigateTo(Type pageType)
    {
        var page = (Page)_serviceProvider.GetRequiredService(pageType);
        _pageServices.ChangePage(page);
    }
}