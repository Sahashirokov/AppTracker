using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
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
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                ViewModelLocator.Init();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Startup failed: {ex}");
                throw;
            }
               
                base.OnStartup(e);
           
        }
    }
}