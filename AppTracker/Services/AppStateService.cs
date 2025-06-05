using System;
using System.Text;
using LauncherApp.MVVM.Model;

namespace LauncherApp.Services;

public class AppStateService:BaseVm
{
    private string _currentTitle;
    
    public string CurrentTitle
    {
        
        get => _currentTitle;
        set
        {
            _currentTitle = value;
            OnPropertyChanged();
        }
    }
}