using Avalonia.ReactiveUI;
using NapCatScript.Desktop.ViewModels.NetWorkModels;

namespace NapCatScript.Desktop.NetWorkViews;

public partial class ListView : ReactiveUserControl<ListViewModel>
{
    public ListView()
    {
        InitializeComponent();
        //this.Resources.Add("SelectList", Models.NetSelectModel.NetSelectModels);
    }
}