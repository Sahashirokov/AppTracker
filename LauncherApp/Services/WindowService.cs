using System.Windows;

namespace LauncherApp.Services;
public interface IWindowService
{
    void Minimize();
    void Close();
    void DragMove();
}
public class WindowService:IWindowService
{
    public void Minimize()
    {
        if (Application.Current.MainWindow != null) Application.Current.MainWindow.WindowState = WindowState.Minimized;
    }

    public void Close()
    {
        if (Application.Current.MainWindow != null) Application.Current.MainWindow.Close();
    }

    public void DragMove()
    {
        Application.Current.Dispatcher.Invoke(() => 
        {
            if (Application.Current.MainWindow?.WindowState != WindowState.Minimized)
            {
                Application.Current.MainWindow?.DragMove();
            }
        });
    }
}