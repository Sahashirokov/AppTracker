using System;
using System.Windows.Controls;

namespace LauncherApp.Services;

public class PageServices
{
    public event Action<Page> OnPageChanged;
    
    public void ChangePage(Page page)=>OnPageChanged?.Invoke(page);
}