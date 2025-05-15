using NapCatScript.Desktop.ViewModels.NetWorkModels;
using ReactiveUI;
using System;
using System.Reactive;

namespace NapCatScript.Desktop.ViewModels;

public class NetWorkViewModel : ViewModelBase
{
    private ViewModelBase _currView;
    public IReactiveCommand OpenNewNetWorkConfigCommand { get; set; }
    public IReactiveCommand OpenWorkListCommand { get; set; }
    public ViewModelBase CurrView { get => _currView; set => this.RaiseAndSetIfChanged(ref _currView, value); }
    private ViewModelBase? _createViewModel;
    private ViewModelBase CreateViewModel
    {
        get {
            return _createViewModel ??= new NetWorkCreateViewModel();
        }
    }
    
    private ViewModelBase? _listViewModel;
    private ViewModelBase ListViewModel
    {
        get {
            return _listViewModel ??= new ListViewModel();
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
        var t = (ListViewModel)ListViewModel;
        t.GetWebUiNetWorkConfig();
    }
}

public static class NetWorkInteraction
{
    /// <summary>
    /// 此交互用于CreateNetWork通知NetWorkViewModel并返回创建对象
    /// </summary>
    public static Interaction<(object, Type), Unit> CreateServerInteraction { get; } = new();
    
    /// <summary>
    /// 此交互用于将MiniView中的更新通知ListView并更新
    /// </summary>
    public static Interaction<(object, Type), Unit> UpdateServerInteraction { get; } = new();
    
    /// <summary>
    /// 此交互用于处理MiniView中的删除
    /// </summary>
    public static Interaction<(object, Type), bool> DeleteServerInteraction { get; } = new();
}