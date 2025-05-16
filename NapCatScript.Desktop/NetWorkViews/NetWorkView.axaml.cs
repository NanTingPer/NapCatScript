using Avalonia.ReactiveUI;
using NapCatScript.Desktop.ViewModels;

namespace NapCatScript.Desktop.NetWorkViews;

public partial class NetWorkView : ReactiveUserControl<NetWorkViewModel>
{
    public NetWorkView()
    {
        InitializeComponent();
    }
}