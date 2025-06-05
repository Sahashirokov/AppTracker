using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using LauncherApp.MVVM.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace LauncherApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private const string MutexName = "Alex_AppTracker_po3sx553097xbkhfspt342";
        private static Mutex _mutex;
        protected override void OnStartup(StartupEventArgs e)
        {
            // Попытка создать мьютекс
            bool createdNew;
            _mutex = new Mutex(true, MutexName, out createdNew);

            // Если мьютекс уже существует - приложение уже запущено
            if (!createdNew)
            {
                MessageBox.Show("Приложение уже запущено", "Ошибка", 
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                Current.Shutdown();
                return;
            }
            try
            {
                ViewModelLocator.Init();
              
            }
            catch (Exception ex)
            {
               // MessageBox.Show($"Startup failed: {ex}");
                Debug.WriteLine($"Startup failed: {ex}");
                throw;
            }
            
            GC.KeepAlive(_mutex);
            base.OnStartup(e);
        }
        protected override void OnExit(ExitEventArgs e)
        {
            _mutex?.ReleaseMutex();
            base.OnExit(e);
        }
    }
}