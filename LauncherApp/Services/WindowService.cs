using System.Windows;

namespace LauncherApp.Services;
// public interface IWindowService
// {
//     void Minimize();
//     void Close();
//     void DragMove();
// }
// public class WindowService:IWindowService
// {
//     private readonly Window _window;
//
//     public WindowService(Window window)
//     {
//         _window = window;
//     }
//
//     public void Minimize() => _window.Dispatcher.Invoke(() => _window.WindowState = WindowState.Minimized);
//     public void Close() => _window.Dispatcher.Invoke(_window.Close);
//     public void DragMove() => _window.Dispatcher.Invoke(_window.DragMove);
// }