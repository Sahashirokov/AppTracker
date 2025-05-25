using System;
using System.Windows.Media;

namespace LauncherApp.MVVM.Model;

public class ApplicationInfo:BaseVm
{
    private string _windowTitle;
    private DateTime _startTime;
    private string _path;
    private ImageSource _icon;
    public int id { get; set; }
    public int ProcessId { get; set; }
    public string Name { get; set; }
    
    public string WindowTitle
    {
        get => _windowTitle;
        set => SetField(ref _windowTitle, value);
    }
    
    public DateTime StartTime
    {
        get => _startTime;
        set
        {
            if (SetField(ref _startTime, value))
            {
                OnPropertyChanged(nameof(Duration));
            }
        }
    }

    public string Path
    {
        get => _path;
        set => SetField(ref _path, value);
    }

    public ImageSource Icon
    {
        get => _icon;
        set => SetField(ref _icon, value);
    }
    
    public TimeSpan Duration => DateTime.Now - StartTime;
    public string? Publisher { get; set; }
    public string? InstallLocation { get; set; }
    public bool IsRunning { get; set; }

    public void RefreshDuration() => RefreshProperty(nameof(Duration));
}