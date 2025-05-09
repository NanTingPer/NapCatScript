using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using NapCatScript.Desktop.ViewModels;

namespace NapCatScript.Desktop.Views;

public partial class NetWorkView : ReactiveUserControl<NetWorkViewModel>
{
    public NetWorkView()
    {
        InitializeComponent();
    }
}