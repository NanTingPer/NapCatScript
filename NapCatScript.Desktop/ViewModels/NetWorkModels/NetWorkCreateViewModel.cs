using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData.Binding;
using NapCatScript.Desktop.Models;
using ReactiveUI;
using Action = System.Action;

namespace NapCatScript.Desktop.ViewModels.NetWorkModels;

public class NetWorkCreateViewModel : ViewModelBase
{
    private ObservableCollection<string> _createList = new ObservableCollection<string>();
    public ObservableCollection<string> CreateList { get => _createList; private set => this.RaiseAndSetIfChanged(ref _createList, value); }
    private NetSelectModel? _selectItem;
    private ViewModelBase? _currentView;
    public NetSelectModel? SelectItem { get => _selectItem; set => this.RaiseAndSetIfChanged(ref _selectItem, value); }
    public ViewModelBase? CurrentView { get => _currentView; set => this.RaiseAndSetIfChanged(ref _currentView, value); }
    public NetWorkCreateViewModel()
    {
        this.WhenAnyValue(@this => @this.SelectItem)
            .Where(f => f is not null)
            .Subscribe(GotoView!);
    }

    private void GotoView(NetSelectModel viewName)
    {
        if (viewName.Name == "Http服务器") {
            CurrentView = new HttpServerViewModel();
        }else if(viewName.Name == "Http客户端") {
            CurrentView = new HttpClientViewModel();
        }
    }
        
}