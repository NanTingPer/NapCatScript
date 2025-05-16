using NapCatScript.Desktop.ViewModels.NetWorkModels;
using NapCatScript.Desktop.Views.NetWorkViews.MiniViews;

namespace NapCatScript.Desktop.NetWorkViews.MiniViews;

public partial class WebSocketClientView : MinView<WebSocketServerViewModel>
{
    public WebSocketClientView()   
    {
        InitializeComponent();
    }
}