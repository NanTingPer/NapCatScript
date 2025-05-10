using Avalonia.ReactiveUI;
using NapCatScript.Desktop.ViewModels.NetWorkModels;

namespace NapCatScript.Desktop.Views.NetWorkViews.MiniViews;

public partial class HttpServerView : ReactiveUserControl<HttpServerViewModel>
{
    public HttpServerView()
    {
        InitializeComponent();
    }
}