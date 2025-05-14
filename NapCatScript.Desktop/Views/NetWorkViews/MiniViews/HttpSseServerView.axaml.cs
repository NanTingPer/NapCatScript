using Avalonia.ReactiveUI;
using NapCatScript.Desktop.ViewModels.NetWorkModels;

namespace NapCatScript.Desktop.Views.NetWorkViews.MiniViews;

public partial class HttpSseServerView : MinView<HttpSseServerViewModel>
{
    public HttpSseServerView()
    {
        InitializeComponent();
    }
}