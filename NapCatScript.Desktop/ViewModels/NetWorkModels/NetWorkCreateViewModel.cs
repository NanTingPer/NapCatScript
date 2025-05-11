using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using DynamicData.Binding;
using ReactiveUI;
using Action = System.Action;

namespace NapCatScript.Desktop.ViewModels.NetWorkModels;

public class NetWorkCreateViewModel : ViewModelBase
{
    private ObservableCollection<string> _createList = new ObservableCollection<string>();
    public ObservableCollection<string> CreateList { get => _createList; private set => this.RaiseAndSetIfChanged(ref _createList, value); }
    private string? _selectItem;
    private ViewModelBase? _currentView;
    public string? SelectItem { get => _selectItem; set => this.RaiseAndSetIfChanged(ref _selectItem, value); }
    public ViewModelBase? CurrentView { get => _currentView; set => this.RaiseAndSetIfChanged(ref _currentView, value); }
    public NetWorkCreateViewModel()
    {
        this.WhenAnyValue(@this => @this.SelectItem)
            .Where(f => f is not null)
            .Subscribe(GotoView!);
        CreateList.Add("Http客户端");
        CreateList.Add("Http服务器");
        CreateList.Add("HttpSSE");
        CreateList.Add("WebSocket客户端");
        CreateList.Add("WebSocket服务器");
    }

    private void GotoView(string viewName)
    {
        if (viewName == "Http服务器") {
            CurrentView = new HttpServerViewModel();
        }else if(viewName == "Http客户端") {
            CurrentView = new HttpClientViewModel();
        }
    }
        
}