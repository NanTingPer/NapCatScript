using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reflection;
using System.Text.Json;
using NapCatScript.Core.JsonFormat;
using NapCatScript.Core.NetWork.NetWorkModel;
using NapCatScript.Core.Services;
using ReactiveUI;
using BindingFlags = System.Reflection.BindingFlags;
using ViewModelType = System.Type;
using ServerType = System.Type;
using System.Collections;

namespace NapCatScript.Desktop.ViewModels.NetWorkModels;

public class ListViewModel : ViewModelBase
{
    private enum ServerTypes
    {
        HttpServer,
        HttpSseServer,
        HttpClient,
        WebSocketClient,
        WebsocketServer
    }

    private static List<ViewModelType> s_viewModelTypes =
    [
        typeof(HttpServerViewModel),
    ];

    private static List<PropertyInfo> s_netWorksPropInfo = [];

    public ObservableCollection<object> NetWorkConfig { get; set; } = [];

    public ListViewModel()
    {
        NetWorkInteraction.CreateServerInteraction.RegisterHandler(ReceiveAddList);
        SetConifg();
    }

    static ListViewModel()
    {
        Type type = typeof(NetWorks);
        var infos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        s_netWorksPropInfo.AddRange(infos);
    }

    public void ReceiveAddList(IInteractionContext<(object, ServerType), Unit> interaction)
    {
        //Handel =>  ‰≥ˆ
        //RegionHandel =>  ‰»Î
        (object obj, ServerType type) input = interaction.Input;
        Add(input.obj, input.type);
        interaction.SetOutput(Unit.Default);

        UpdateListWeb(input.obj, input.type);
        //switch (input.type.Name) {
        //    case nameof(ServerTypes.HttpClient):
        //        break;
        //    case nameof(ServerTypes.HttpServer):
        //        break;
        //    case nameof(ServerTypes.WebSocketClient):
        //        break;
        //    case nameof()
        //}
    }

    public void UpdateListWeb(object obj, ServerType type)
    {
        if (_netWorks is null) SetConifg();
        if (_netWorks is null) return;

        PropertyInfo? targetInfo = s_netWorksPropInfo.FirstOrDefault(f => f.Name.Contains(type.Name));
        if (targetInfo is null) return;
        IList? netWorkList = (IList?)targetInfo.GetValue(_netWorks);
        if (netWorkList is null) return;
        netWorkList.Add(obj);
    }

    private NetWorks? _netWorks;
    public async void SetConifg()
    {
        string s = await Core.MsgHandle.Utils.GetNetWorkConfig("6099", "napcat");
        if(!s.GetJsonElement(out var netWorkJson))
            return;
        if(!netWorkJson.TryGetPropertyValue("network", out var network))
            return;
        try {
            _netWorks = network.Deserialize<NetWorks>()!;
        } catch (Exception e) {
            Loging.Log.Erro("ListViewModel::SetConfig Õ¯¬Á≈‰÷√–Ú¡–ªØ ß∞‹!", e.Message, e.StackTrace);
            return;
        }
        Add(_netWorks);
    }

    private void Add<T>(T obj) => AddAll([obj]);
    private void Add(object obj, Type type) => AddAll([obj], type.Name, type);
    private void AddAll<T>(List<T> list, string? typeName_ = null, Type? type_ = null)
    {
        list.ForEach(f =>
        {
            string typeName;
            if (typeName_ == null)
                typeName = typeof(T).Name;
            else typeName = typeName_;
            ViewModelType? viewType = s_viewModelTypes.FirstOrDefault(f => f.Name.Contains(typeName));
            if (viewType is null)
                return;
            ConstructorInfo ctorInfo = viewType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, [])!;
            object viewModelObj;
            try {
                viewModelObj = ctorInfo!.Invoke([]);
            }
            catch (Exception e) {
                Loging.Log.Erro(e.Message, e.StackTrace);
                return;
            }
            
            Core.Utils.TypeMap(viewType, typeof(T), viewModelObj, f);
            NetWorkConfig.Add(viewModelObj);
        });
    }

    private void Add(NetWorks config)
    {
        AddAll(config.HttpClients);
        AddAll(config.HttpServers);
        AddAll(config.HttpSseServers);
        AddAll(config.WebSocketClients);
        AddAll(config.WebSocketServers);
    }
}