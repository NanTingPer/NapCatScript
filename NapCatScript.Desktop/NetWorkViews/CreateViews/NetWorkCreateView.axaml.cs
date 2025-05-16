using Avalonia.ReactiveUI;
using NapCatScript.Desktop.ViewModels.NetWorkModels;

namespace NapCatScript.Desktop.NetWorkViews.CreateViews;

public partial class NetWorkCreateView : ReactiveUserControl<NetWorkCreateViewModel>
{
    public NetWorkCreateView()
    {
        InitializeComponent();
    }
}