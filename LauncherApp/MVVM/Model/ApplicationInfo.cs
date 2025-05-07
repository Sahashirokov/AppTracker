using System;
using System.Windows.Media;

namespace LauncherApp.MVVM.Model;

public class ApplicationInfo
{
    
    public int ProcessId { get; set; }
    public string Name { get; set; }
    public string WindowTitle { get; set; }
    
    public string? Path { get; set; }
    public DateTime StartTime { get; set; }
    public ImageSource Icon { get; set; }
    
    public TimeSpan Duration => DateTime.Now - StartTime;
}