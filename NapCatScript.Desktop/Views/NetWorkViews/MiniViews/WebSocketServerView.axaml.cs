using Avalonia.ReactiveUI;
using NapCatScript.Desktop.ViewModels.NetWorkModels;

namespace NapCatScript.Desktop.Views.NetWorkViews.MiniViews;

public partial class WebSocketServerView : ReactiveUserControl<WebSocketServerViewModel>
{
    public WebSocketServerView()
    {
        InitializeComponent();
    }
}