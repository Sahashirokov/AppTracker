using LauncherApp.MVVM.Model;

namespace LauncherApp.MVVM.ViewModel;

public class ApplicationInfoWrapper:BaseVm
{
    private bool _isFavorite;
    
    public ApplicationInfo Info { get; }
    public bool IsFavorite
    {
        get => _isFavorite;
        set => SetField(ref _isFavorite, value);
    }

    public ApplicationInfoWrapper(ApplicationInfo info, bool isFavorite)
    {
        Info = info;
        IsFavorite = isFavorite;
    }
    
}