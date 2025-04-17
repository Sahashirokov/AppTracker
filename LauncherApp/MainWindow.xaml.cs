using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LauncherApp.Model;

namespace LauncherApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        //Loaded="MainWindow_OnLoaded"
       /* private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            _appList = new BindingList<AppChecker>()
            {
                new AppChecker(){ Id = "One"},
                new AppChecker(){ Id = "Two"}
            };
            DgAppList.ItemsSource = _appList;
            
        }*/
        /* private void Add_OnClick(object sender, RoutedEventArgs e)
        {
            AppChecker appChecker = new AppChecker()
            {
                Id = "Hello"
            };
            _appList.Add(appChecker);
        }
        private void Start_OnClick(object sender, RoutedEventArgs e)
        {
          
            MessageBox.Show("Запущен");
        }

        private void Edit_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Изменен");
        }

        private void Delete_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Удален");
        }*/


        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}