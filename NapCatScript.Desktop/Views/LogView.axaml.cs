using System;
using Avalonia.ReactiveUI;
using NapCatScript.Desktop.ViewModels;

namespace NapCatScript.Desktop.Views;

public partial class LogView : ReactiveUserControl<LogViewModel>
{
    public LogView()
    {
        InitializeComponent();
    }
}