using Avalonia.ReactiveUI;
using NapCatScript.Desktop.ViewModels.NetWorkModels;

namespace NapCatScript.Desktop.NetWorkViews.CreateViews;

public partial class CreateHttpClientView : ReactiveUserControl<HttpClientViewModel>
{
    public CreateHttpClientView()
    {
        InitializeComponent();
    }
}