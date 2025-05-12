using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using NapCatScript.Desktop.ViewModels;
using NapCatScript.Desktop.ViewModels.NetWorkModels;

namespace NapCatScript.Desktop.Views.NetWorkViews;

public partial class ListView : ReactiveUserControl<ListViewModel>
{
    public ListView()
    {
        InitializeComponent();
        //this.Resources.Add("SelectList", Models.NetSelectModel.NetSelectModels);
    }
}