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
    private readonly IWindowService _windowService;
    private Page _pageSource;
    
    private readonly Dictionary<Type, string> _pageTitles = new()
    {
        { typeof(FavoritePage), "Избранное" },
        { typeof(AllAppsPage), "Все приложения" }
    };
    public ICommand MinimizeCommand { get; }
    public ICommand CloseCommand { get; }
    public ICommand DragMoveCommand { get; }
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

    public MainViewModel(INavigationService navigation,PageServices pageServices,IWindowService windowService)
    {
        _navigation = navigation;
        _windowService = windowService;
        pageServices.OnPageChanged += page => PageSource = page;
        MinimizeCommand = new DelegateCommand(() => _windowService.Minimize());
        CloseCommand = new DelegateCommand(() => _windowService.Close());
        DragMoveCommand = new DelegateCommand<MouseButtonEventArgs>(e =>
        {
            if (e == null) return;
            if (e.ChangedButton != MouseButton.Left) return;
            if (e.ClickCount != 1) return;
    
            try
            {
                _windowService.DragMove();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"DragMove error: {ex.Message}");
            }
        });
        _navigation.NavigateTo<FavoritePage>();
    }
    
    private void UpdateTitle()
    {
        if (PageSource != null && _pageTitles.TryGetValue(PageSource.GetType(), out var title))
            Title = title;
    }
    public ICommand NavigateToFavoriteCommand => new DelegateCommand(() => 
        _navigation.NavigateTo<FavoritePage>());

    public ICommand NavigateToAllAppsCommand => new DelegateCommand(() =>
            _navigation.NavigateTo<AllAppsPage>()
        );
    
}