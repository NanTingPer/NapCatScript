using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NapCatScript.Desktop.ViewModels.NetWorkModels;

namespace NapCatScript.Desktop.ChatViews;

public partial class LinkWebSocketServerView : ChatView<LinkWebSocketServerView, WebSocketServerViewModel>
{
    public LinkWebSocketServerView()
    {
        InitializeComponent();
    }
}