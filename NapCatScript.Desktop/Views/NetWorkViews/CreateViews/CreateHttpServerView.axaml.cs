using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using NapCatScript.Desktop.ViewModels.NetWorkModels;

namespace NapCatScript.Desktop.Views.NetWorkViews.CreateViews;

public partial class CreateHttpServerView : ReactiveUserControl<HttpServerViewModel>
{
    public CreateHttpServerView()
    {
        InitializeComponent();
    }
}