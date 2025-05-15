using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using NapCatScript.Desktop.ViewModels;

namespace NapCatScript.Desktop.Views.LoginView;

public partial class WebUiLoginView : ReactiveUserControl<WebUiLoginInfo>
{
    public WebUiLoginView()
    {
        InitializeComponent();
    }
}