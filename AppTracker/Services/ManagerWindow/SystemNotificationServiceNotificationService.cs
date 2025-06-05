using System;
using System.Windows.Forms;
using System.Windows.Threading;
using Microsoft.Toolkit.Uwp.Notifications;
using Application = System.Windows.Application;

namespace LauncherApp.Services.ManagerWindow;

public class SystemNotificationServiceNotificationService : ISystemNotificationServiceNotificationService
{
    public void ShowNotification(string title, string message)
    {
        // Для Windows 10/11 используем современные тосты
        if (Environment.OSVersion.Version.Major >= 10)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                var notifyIcon = new NotifyIcon
                {
                    Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName),
                    Visible = true
                };

                notifyIcon.ShowBalloonTip(
                    3000,
                    title,
                    message,
                    ToolTipIcon.Info);

                notifyIcon.BalloonTipClosed += (s, e) => 
                {
                    notifyIcon.Dispose();
                };
            });
        }
        else
        {
            // Для старых версий Windows используем balloon tips
            ShowBalloonTip(title, message);
        }
    }

    private void ShowBalloonTip(string title, string message)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            // Создаем иконку в системном трее
            var notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName),
                Visible = true
            };
            
            notifyIcon.ShowBalloonTip(
                3000,// 5 секунд
                title,
                message,
                System.Windows.Forms.ToolTipIcon.Info);

            // Автоматически убираем иконку после показа
            notifyIcon.BalloonTipClosed += (s, e) => 
            {
                notifyIcon.Visible = false;
                notifyIcon.Dispose();
            };
        });
    }

    public void ShowNotification(string message)
    {
        throw new NotImplementedException();
    }
}