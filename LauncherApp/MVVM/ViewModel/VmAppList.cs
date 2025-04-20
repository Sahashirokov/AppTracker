using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Input;
using LauncherApp.Command;
using LauncherApp.Model;
using LauncherApp.MVVM.Model;

namespace LauncherApp.ViewModel;

public class VmAppList:BaseVm
{
    public ObservableCollection<AppChecker> AppCheckers { get; set; }
    public VmAppList()
    {
        AppCheckers = new ObservableCollection<AppChecker>
        {
            new AppChecker(){ Id = "1"},
            new AppChecker(){ Id = "2"},
            new AppChecker(){ Id = "3"},
        };
        AppCheckers.CollectionChanged += (s, e) => 
        {
            for (int i = 0; i < AppCheckers.Count; i++)
            {
                AppCheckers[i].Id = $"{i + 1}";
            }
        };
    }
    public ICommand ToggleCommand => new DelegateCommand<AppChecker>(item => 
    {
        item.IsRunning = !item.IsRunning;
    });
    public ICommand AddCommand => new DelegateCommand(
        () => 
        {
            try
            {
                Console.WriteLine("Добавление элемента...");

                var newItem = new AppChecker();
                
               
                AppCheckers.Add(newItem);
            
                Console.WriteLine($"Элемент '{newItem.Id}' успешно добавлен. Всего элементов: {AppCheckers.Count}");
                OnPropertyChanged("AddCommand");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при добавлении: {ex.Message}");
            }
        });
    private string? _name;
    
    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged("Id");
            }
        }
    }
    
    public ICommand Edit
    {
        get
        {
            return new DelegateCommand<AppChecker>((obj =>
            {
                MessageBox.Show("Изменен");
            }));
        }
    }
    public DelegateCommand<AppChecker> Remove => new DelegateCommand<AppChecker>((item) =>
    {
        AppCheckers.Remove(item);
    },(item)=>item != null);

}