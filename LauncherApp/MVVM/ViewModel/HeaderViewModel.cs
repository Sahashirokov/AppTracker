using System.Windows.Input;
using LauncherApp.Command;
using LauncherApp.MVVM.Model;
using LauncherApp.Pages;
using LauncherApp.Services;

namespace LauncherApp.MVVM.ViewModel;

public class HeaderViewModel:BaseVm
{
    private readonly IWindowService _windowService;
    private readonly INavigationService _navigationService;
    public HeaderViewModel(
        IWindowService windowService,
        INavigationService navigationService)
    {
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

    public ICommand MinimizeCommand { get; }
    public ICommand CloseCommand { get; }
    public ICommand DragMoveCommand { get; }
    public ICommand NavigateToFavoriteCommand => 
        new DelegateCommand(() => _navigationService.NavigateTo<FavoritePage>());
    public ICommand NavigateToAllAppsCommand => 
        new DelegateCommand(() => _navigationService.NavigateTo<AllAppsPage>());
    
}