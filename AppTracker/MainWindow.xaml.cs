using System;
using System.Windows;

namespace LauncherApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        private void HeaderControl_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}