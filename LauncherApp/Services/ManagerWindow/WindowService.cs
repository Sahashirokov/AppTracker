using System.Drawing;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;
using FontStyle = System.Drawing.FontStyle;

namespace LauncherApp.Services.ManagerWindow;
public interface IWindowService
{
    void Minimize();
    void Close();
    void DragMove();
}
public class WindowService:IWindowService
{
    private NotifyIcon _trayIcon;
    private bool _isInitialized;
   public WindowService()
   {
       Application.Current.Exit += (s, e) => DisposeTrayIcon();
   }

   private void InitializeTrayIcon()
   {
       if (_isInitialized) return;
        
       _trayIcon = new NotifyIcon();
       _trayIcon.Icon = Icon.ExtractAssociatedIcon(Assembly.GetEntryAssembly().Location);
       _trayIcon.Text = "AppTracker";
       _trayIcon.Visible = true;
       
       var menu = new ContextMenuStrip();
       menu.Renderer = new CustomMenuRenderer();
       
       menu.Padding = new Padding(10);
       menu.BackColor = Color.DarkSlateBlue;
       menu.ForeColor = Color.White;
       menu.Font = new Font("Segoe UI", 10, FontStyle.Regular);
       
       var openItem = new ToolStripMenuItem("Открыть");
       var exitItem = new ToolStripMenuItem("Закрыть");
       
       const int verticalPadding = 8;
       const int horizontalPadding = 15;
       openItem.Padding = new Padding(horizontalPadding, verticalPadding, horizontalPadding, verticalPadding);
       exitItem.Padding = new Padding(horizontalPadding, verticalPadding, horizontalPadding, verticalPadding);
       openItem.MouseEnter += (s, e) => openItem.ForeColor = Color.Black;
       openItem.MouseLeave += (s, e) => openItem.ForeColor = Color.White;
       exitItem.MouseEnter += (s, e) => exitItem.ForeColor = Color.Black;
       exitItem.MouseLeave += (s, e) => exitItem.ForeColor = Color.White;
       
       openItem.Click += (s, e) => ShowMainWindow();
       exitItem.Click += (s, e) => RealClose();
    
       menu.Items.Add(openItem);
       menu.Items.Add(exitItem);
       menu.ShowImageMargin = false;
       menu.Items.Add(new ToolStripSeparator());
       _trayIcon.ContextMenuStrip = menu;
        
       _trayIcon.DoubleClick += (s, e) => ShowMainWindow();
       _isInitialized = true;
   }

   private void ShowMainWindow()
   {
       if (Application.Current.MainWindow != null)
       {
           Application.Current.MainWindow.Show();
           Application.Current.MainWindow.WindowState = WindowState.Normal;
           Application.Current.MainWindow.Activate();
           if (_trayIcon != null)
           {
               _trayIcon.Visible = false;
           }
       }
   }

   private void RealClose()
   {
       DisposeTrayIcon();
       Application.Current.MainWindow?.Close();
   }

   private void DisposeTrayIcon()
   {
       if (_trayIcon == null) return;
       _trayIcon.Visible = false;
       _trayIcon.Dispose();
       _trayIcon = null;
       _isInitialized = false;
   }
   public void Close()
   {
       if (Application.Current.MainWindow == null) return;
        
       InitializeTrayIcon();
       _trayIcon.Visible = true;
       Application.Current.MainWindow.Hide();
   }
    public void Minimize()
    {
        if (Application.Current.MainWindow != null) Application.Current.MainWindow.WindowState = WindowState.Minimized;
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