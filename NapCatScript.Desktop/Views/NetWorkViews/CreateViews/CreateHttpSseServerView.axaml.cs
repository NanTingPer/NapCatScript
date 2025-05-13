using Avalonia.ReactiveUI;
using NapCatScript.Desktop.ViewModels.NetWorkModels;

namespace NapCatScript.Desktop.Views.NetWorkViews.CreateViews;

public partial class CreateHttpSseServerView : ReactiveUserControl<HttpSseServerViewModel>
{
    public CreateHttpSseServerView()
    {
        InitializeComponent();
    }
}