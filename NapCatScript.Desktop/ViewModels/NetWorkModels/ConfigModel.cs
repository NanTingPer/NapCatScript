using NapCatScript.Core.NetWork.NetWorkModel;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using ReactiveUI;

namespace NapCatScript.Desktop.ViewModels.NetWorkModels;

/// <summary>
/// <para> 网络配置ViewModel基类 </para>
/// </summary>
/// <typeparam name="TThis"> 实现类自身 </typeparam>
/// <typeparam name="TServer"> 此类所使用的<see cref="NetWorkModels"/> </typeparam>
public abstract class ConfigModel<TThis, TServer> : ViewModelBase 
    where TServer : new()
    where TThis : ViewModelBase
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
        this.WhenAnyValue(@this => @this.AddNetWorkCommand);
        TServer config = new TServer();
        Core.Utils.TypeMap(Type, ServerType, this, config);
        NetWorkInteraction.CreateServerInteraction.Handle((config, ServerType)).Subscribe();
    }
}
