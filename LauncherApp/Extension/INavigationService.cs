using System;
using System.Windows.Controls;

namespace LauncherApp.Services;

public interface INavigationService
{
    void NavigateTo<T>() where T : Page;
    void NavigateTo(Type pageType);
}