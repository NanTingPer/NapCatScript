using Avalonia.ReactiveUI;
using NapCatScript.Desktop.ViewModels.NetWorkModels;

namespace NapCatScript.Desktop.NetWorkViews.CreateViews;

public partial class CreateHttpServerView : ReactiveUserControl<HttpServerViewModel>
{
    public CreateHttpServerView()
    {
        InitializeComponent();
    }
}