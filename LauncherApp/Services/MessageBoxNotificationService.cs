using System.Windows;

namespace LauncherApp.Services;

public interface INotificationService
{
    void ShowSuccess(string message);
    void ShowError(string message);
}
public class MessageBoxNotificationService: INotificationService
{
    public void ShowSuccess(string message) 
        => MessageBox.Show(message, "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

    public void ShowError(string message) 
        => MessageBox.Show(message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
}