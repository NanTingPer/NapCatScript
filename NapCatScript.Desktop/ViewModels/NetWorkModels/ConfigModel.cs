using NapCatScript.Core.NetWork.NetWorkModel;
using System;

namespace NapCatScript.Desktop.ViewModels.NetWorkModels;

public abstract class ConfigModel<TThis, TServer> : ViewModelBase where TServer : new()
{
    public static Type Type { get; } = typeof(TThis);
    protected static Type ServerType { get; } = typeof(TServer);

    public void CreateNetWork()
    {
        TServer config = new TServer();
        Core.Utils.TypeMap(Type, ServerType, this, config);
        NetWorkInteraction.CreateServerInteraction.Handle((config, ServerType)).Subscribe().Dispose();
    }
}
