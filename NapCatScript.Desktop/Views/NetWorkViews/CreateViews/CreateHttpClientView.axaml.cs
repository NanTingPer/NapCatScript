using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using NapCatScript.Core.NetWork.NetWorkModel;
using NapCatScript.Desktop.ViewModels.NetWorkModels;
using ReactiveUI;

namespace NapCatScript.Desktop.Views.NetWorkViews.CreateViews;

public partial class CreateHttpClientView : ReactiveUserControl<HttpClientViewModel>
{
    public CreateHttpClientView()
    {
        InitializeComponent();
        /*this.WhenActivated(act =>
        {
            act.Invoke(HttpServerViewModel.CreateServerInteraction.RegisterHandler(HandelMethod));
        });*/
    }

    /*private void HandelMethod(IInteractionContext<HttpServerViewModel, HttpServer> interaction)
    {
        var r = interaction.Input;
    }*/
}