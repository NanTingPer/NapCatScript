using Avalonia.ReactiveUI;
using NapCatScript.Desktop.ViewModels.NetWorkModels;

namespace NapCatScript.Desktop.NetWorkViews.CreateViews;

public partial class CreateWebSocketServerView : ReactiveUserControl<WebSocketServerViewModel>
{
    public CreateWebSocketServerView()
    {
        InitializeComponent();
    }
}