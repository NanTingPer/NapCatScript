using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using NapCatScript.Core.NetWork.NetWorkModel;
using NapCatScript.Desktop.ViewModels.NetWorkModels;
using ReactiveUI;

namespace NapCatScript.Desktop.Views.NetWorkViews.CreateViews;

public partial class CreateWebSocketClientView : ReactiveUserControl<WebSocketClientViewModel>
{
    public CreateWebSocketClientView()
    {
        InitializeComponent();
    }
}