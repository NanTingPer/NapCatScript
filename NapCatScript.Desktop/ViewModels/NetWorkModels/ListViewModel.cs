using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using NapCatScript.Core.JsonFormat;
using NapCatScript.Core.NetWork.NetWorkModel;
using NapCatScript.Core.Services;
using BindingFlags = System.Reflection.BindingFlags;
using ViewModelType = System.Type;

namespace NapCatScript.Desktop.ViewModels.NetWorkModels;

public class ListViewModel : ViewModelBase
{
    private List<ViewModelType> Types =
    [
        typeof(HttpServerViewModel),
    ];
    
    public ObservableCollection<object> NetWorkConfig { get; set; } = [];

    public ListViewModel()
    {
        SetConifg();
    }

    public async void SetConifg()
    {
        string s = await NapCatScript.Core.MsgHandle.Utils.GetNetWorkConfig("6099", "napcat");
        if(!s.GetJsonElement(out var netWorkJson))
            return;
        if(!netWorkJson.TryGetPropertyValue("network", out var network))
            return;

        NetWorks config = network.Deserialize<NetWorks>()!;
        Add(config);
    }
    
    public void AddAll<T>(List<T> list)
    {
        list.ForEach(f =>
        {
            string typeName = typeof(T).Name;
            ViewModelType? viewType = Types.FirstOrDefault(f => f.Name.Contains(typeName));
            if (viewType is null)
                return;
            ConstructorInfo ctorInfo = viewType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, []);
            object viewModelObj;
            try {
                viewModelObj = ctorInfo.Invoke([]);
            }
            catch (Exception e) {
                Loging.Log.Erro(e.Message, e.StackTrace);
                return;
            }
            
            Core.Utils.TypeMap(viewType, typeof(T), viewModelObj, f);
            NetWorkConfig.Add(viewModelObj);
        });
    }

    public void Add(NetWorks config)
    {
        AddAll(config.HttpClients);
        AddAll(config.HttpServers);
        AddAll(config.HttpSseServers);
        AddAll(config.WebSocketClients);
        AddAll(config.WebSocketServers);
    }
}