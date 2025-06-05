using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using LauncherApp.Command;
using LauncherApp.MVVM.Model;
using LauncherApp.Services;
using  LauncherApp.MVVM.View.Pages;

namespace LauncherApp.MVVM.ViewModel;

public class MainViewModel : BaseVm
{
    private readonly INavigationService _navigation;
    private Page _pageSource;
    private readonly AppStateService _appState;
    private readonly Dictionary<Type, string> _pageTitles = new()
    {
        { typeof(FavoritePage), "Избранное" },
        { typeof(AllAppsPage), "Все приложения" }
    };

    private string _title;

    public Page PageSource
    {
        get => _pageSource;
        set
        {
            _pageSource = value;
            OnPropertyChanged();
        }
    }
    public MainViewModel(INavigationService navigation,PageServices pageServices,AppStateService appState)
    {
        _appState = appState;
        _navigation = navigation;
        pageServices.OnPageChanged += page =>
        {
            PageSource = page;
            UpdateTitle();
        };
        _navigation.NavigateTo<FavoritePage>();
        
    }

    public string Title
    {
        get => _title;
        set { _title = value; OnPropertyChanged(); }
    }

    private void UpdateTitle()
    {
        if (PageSource != null && _pageTitles.TryGetValue(PageSource.GetType(), out var title))
        {
            _appState.CurrentTitle = title;
            Title = title;
        }
            
    }
    
}