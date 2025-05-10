using NapCatScript.Core.NetWork.NetWorkModel;
using NapCatScript.Desktop.ViewModels.NetWorkModels;
using ReactiveUI;

namespace NapCatScript.Desktop.ViewModels;

public class NetWorkViewModel : ViewModelBase
{
    public static Interaction<HttpServer, HttpServerViewModel> CreateHttpServerInteraction = new ();
    private ViewModelBase _currView;
    public IReactiveCommand OpenNewNetWorkConfigCommand { get; set; }
    public IReactiveCommand OpenWorkListCommand { get; set; }
    public ViewModelBase CurrView { get=> _currView; set => this.RaiseAndSetIfChanged(ref _currView, value); }
    private ViewModelBase? _createViewModel;
    private ViewModelBase CreateViewModel
    {
        get {
            if (_createViewModel is null) {
                _createViewModel = new NetWorkCreateViewModel();
                return _createViewModel;
            }
            return _createViewModel;
        }
    }
    
    private ViewModelBase? _listViewModel;
    private ViewModelBase ListViewModel
    {
        get {
            if (_listViewModel is null) {
                _listViewModel = new ListViewModel();
                return _listViewModel;
            }
            return _listViewModel;
        }
    }
    public NetWorkViewModel()
    {
        OpenNewNetWorkConfigCommand = ReactiveCommand.Create(OpenNewNetWorkConfig);
        OpenWorkListCommand = ReactiveCommand.Create(OpenWorkList);
        CurrView = ListViewModel;
    }

    /// <summary>
    /// 打开新建网络配置
    /// </summary>
    private void OpenNewNetWorkConfig()
    {
        CurrView = CreateViewModel;
    }

    private void OpenWorkList()
    {
        CurrView = ListViewModel;
    }
}