using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LauncherApp.Command;
using LauncherApp.ViewModel;

namespace LauncherApp.Model;

public class AppChecker:BaseVm
{
    private readonly VmAppList _vmAppList;
    private string _id;
    public string Id
    {
        get=>_id;
        set
        {
            _id = value;
            OnPropertyChanged();
        }
        
    }
    private bool _isRunning;
    public bool IsRunning
    {
        get => _isRunning;
        set
        {
            _isRunning = value;
            OnPropertyChanged();
        }
    }
    
}