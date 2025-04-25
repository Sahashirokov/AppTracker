using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;
using LauncherApp.Command;
using LauncherApp.MVVM.Model;
using LauncherApp.Pages;
using LauncherApp.Services;

namespace LauncherApp.MVVM.ViewModel;

public class MainViewModel : BaseVm
{
    private readonly INavigationService _navigation;
    private Page _pageSource;
    
    private readonly Dictionary<Type, string> _pageTitles = new()
    {
        { typeof(FavoritePage), "Избранное" },
        { typeof(AllAppsPage), "Все приложения" }
    };
    public Page PageSource
    {
        get => _pageSource;
        set
        {
            _pageSource = value;
            OnPropertyChanged();
            UpdateTitle();
        }
    }
    private string _title;
    
    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged();
           
        }
    }

    public MainViewModel(INavigationService navigation,PageServices pageServices)
    {
        _navigation = navigation;
        pageServices.OnPageChanged += page => PageSource = page;
        _navigation.NavigateTo<FavoritePage>();
    }
    
    private void UpdateTitle()
    {
        if (PageSource != null && _pageTitles.TryGetValue(PageSource.GetType(), out var title))
            Title = title;
    }
    
}