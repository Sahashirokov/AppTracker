using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LauncherApp.Command;
using LauncherApp.Model;
using LauncherApp.MVVM.Model;
using LauncherApp.Pages;
using LauncherApp.Services;

namespace LauncherApp.MVVM.ViewModel;

public class MainViewModel : BaseVm
{
    private readonly INavigationService _navigation;
   // private readonly IWindowService _windowService;
    private Page _pageSource;
    
    public Page PageSource
    {
        get => _pageSource;
        set
        {
            _pageSource = value;
            OnPropertyChanged();
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
        //_windowService = windowService;
        pageServices.OnPageChanged += page => PageSource = page;
        _navigation.NavigateTo<FavoritePage>();
        NavigateToInitial();
    }

    private void NavigateToInitial()
    {
        _navigation.NavigateTo<FavoritePage>();
    }

    public ICommand NavigateToFavoriteCommand => new DelegateCommand(() => 
        _navigation.NavigateTo<FavoritePage>());

    public ICommand NavigateToAllAppsCommand => new DelegateCommand<MainViewModel>((item) =>
        {
            _navigation.NavigateTo<AllAppsPage>();
            Console.WriteLine(item);
        }
        );

    // public ICommand MinimizeCommand => new DelegateCommand(() => _windowService.Minimize());
    // public ICommand CloseCommand => new DelegateCommand(() => _windowService.Close());
    // public ICommand DragMoveCommand => new DelegateCommand(() => _windowService.DragMove());
}