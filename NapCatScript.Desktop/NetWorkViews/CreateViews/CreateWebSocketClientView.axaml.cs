using Avalonia.ReactiveUI;
using NapCatScript.Desktop.ViewModels.NetWorkModels;

namespace NapCatScript.Desktop.NetWorkViews.CreateViews;

public partial class CreateWebSocketClientView : ReactiveUserControl<WebSocketClientViewModel>
{
    public CreateWebSocketClientView()
    {
        InitializeComponent();
    }
}