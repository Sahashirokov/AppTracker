using System;
using System.Windows.Input;
using LauncherApp.Command;
using LauncherApp.MVVM.Model;
using LauncherApp.Services;
using LauncherApp.MVVM.View.Pages;
using LauncherApp.Services.ManagerWindow;

namespace LauncherApp.MVVM.ViewModel;

public class HeaderViewModel:BaseVm
{
    private readonly IWindowService _windowService;
    private readonly INavigationService _navigationService;
    private readonly AppStateService _appState;
    private IDisposable _subscription;
    private string _title;
    
    public HeaderViewModel(
        IWindowService windowService,
        INavigationService navigationService,AppStateService appState)
    {
        _appState = appState;
        _subscription = _appState.SubscribeToProperty(
            x => x.CurrentTitle,
            () => Title = _appState.CurrentTitle
        );
        _windowService = windowService;
        _navigationService = navigationService;
        
        
        MinimizeCommand = new DelegateCommand(() => _windowService.Minimize());
        CloseCommand = new DelegateCommand(() => _windowService.Close());
        DragMoveCommand = new DelegateCommand<MouseButtonEventArgs>(OnDragMove);
       
    }
    
    private void OnDragMove(MouseButtonEventArgs e)
    {
        if (e?.ChangedButton == MouseButton.Left && e.ClickCount == 1)
        {
            _windowService.DragMove();
        }
    }
    public string Title
    {
        get => _title;
        set => SetField(ref _title, value);
    }
    public override void Dispose()
    {
        _subscription?.Dispose();
        base.Dispose();
    }

    public ICommand MinimizeCommand { get; }
    public ICommand CloseCommand { get; }
    public ICommand DragMoveCommand { get; }
    public ICommand NavigateToFavoriteCommand => 
        new DelegateCommand(() => _navigationService.NavigateTo<FavoritePage>());
    public ICommand NavigateToAllAppsCommand => 
        new DelegateCommand(() => _navigationService.NavigateTo<AllAppsPage>());
    
}