using Avalonia.ReactiveUI;
using NapCatScript.Desktop.ViewModels.NetWorkModels;

namespace NapCatScript.Desktop.Views.NetWorkViews.MiniViews;

public partial class HttpClientView : ReactiveUserControl<HttpClientViewModel>
{
    public HttpClientView()
    {
        InitializeComponent();
    }
}