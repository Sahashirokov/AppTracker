
using System;
using System.Windows.Media;

namespace LauncherApp.MVVM.Model;

public class AppM
{
   public int Id{get;set;}
    
   public string Name {get; set;}
    
   public string Version {get; set;}
   
   public string WindowTitle {get; set;}
   
   public string Path {get; set;}
   //TODO мб убрать лишнее Icon поле
   public string Icon { get; set; }
   public DateTime StartTime { get; set; }
   
   public DateTime LastTime { get; set; }
   
   public TimeSpan DurationCurrent => DateTime.Now - StartTime;
   
   public TimeSpan TotalTime {get; set;}
   
   public TimeSpan? forWeek { get; set; } 
}