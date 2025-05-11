using NapCatScript.Core.NetWork.NetWorkModel;
using System;
using System.Collections.ObjectModel;
using ReactiveUI;

namespace NapCatScript.Desktop.ViewModels.NetWorkModels;

public abstract class ConfigModel<TThis, TServer> : ViewModelBase where TServer : new()
{
    protected ConfigModel()
    {
        AddNetWorkCommand = ReactiveCommand.Create(CreateNetWork);
    }
    /// <summary>
    /// 创建
    /// </summary>
    public IReactiveCommand AddNetWorkCommand { get; set; }
    public ObservableCollection<string> FormatValue { get; } = ["string", "array"];
    public static Type Type { get; } = typeof(TThis);
    protected static Type ServerType { get; } = typeof(TServer);

    public void CreateNetWork()
    {
        TServer config = new TServer();
        Core.Utils.TypeMap(Type, ServerType, this, config);
        NetWorkInteraction.CreateServerInteraction.Handle((config, ServerType)).Subscribe();
    }
}
