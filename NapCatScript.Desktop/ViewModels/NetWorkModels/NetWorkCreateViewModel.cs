using System.Collections.ObjectModel;
using ReactiveUI;

namespace NapCatScript.Desktop.ViewModels.NetWorkModels;

public class NetWorkCreateViewModel : ViewModelBase
{
    private ObservableCollection<string> _createList = new ObservableCollection<string>();
    public ObservableCollection<string> CreateList { get => _createList; private set => this.RaiseAndSetIfChanged(ref _createList, value); }

    public NetWorkCreateViewModel()
    {
        CreateList.Add("Http客户端");
        CreateList.Add("Http服务器");
        CreateList.Add("HttpSSE");
        CreateList.Add("WebSocket客户端");
        CreateList.Add("WebSocket服务器");
    }
}