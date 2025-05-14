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
using System.Threading.Tasks;
using NapCatScript.Core.MsgHandle;
using NapCatScript.Desktop.Models;

using ViewModelObject = System.Object;
using ServerObject = System.Object;

namespace NapCatScript.Desktop.ViewModels.NetWorkModels;

/// <summary>
/// <para> 如添加了MiniView 需要在这边进行注册 </para>
/// <para> 如添加了MiniView 需要前往<see cref="ViewLocator"/>注册 </para>
/// </summary>
public class ListViewModel : ViewModelBase
{
    private string _musicSignUrl = "";
    private bool _enableLocalFile2Url = false;
    private bool _parseMultMsg = false;
    private NetWorks? _netWorks;
    private NetSelectModel? _selectedModel;
    
    /// <summary>
    /// object => ViewModel, View => MinView
    /// </summary>
    private ObservableCollection<ViewModelObject> allNetWorkConfig = [];
    
    private static List<PropertyInfo> s_netWorksPropInfo = [];
    private static List<ViewModelType> s_viewModelTypes =
    [
        HttpServerViewModel.Type,
        HttpClientViewModel.Type,
        WebSocketServerViewModel.Type,
        HttpSseServerViewModel.Type,
        WebSocketClientViewModel.Type,
    ];
    
    /// <summary>
    /// <para> 当前应当被显示的网络配置项来自<see cref="allNetWorkConfig"/> </para>
    /// <para> 通过<see cref="SelectList"/> 更新 </para> 
    /// </summary>
    public ObservableCollection<ViewModelObject> NetWorkConfig { get; set; } = []; //NetWorkConfig
    public NetSelectModel? SelectedModel {get => _selectedModel; set => this.RaiseAndSetIfChanged(ref _selectedModel, value); }
    
    public ListViewModel()
    {
        GetWebUiNetWorkConfig(); //从WebUI拉取配置
        this.WhenAnyValue(@this => @this.SelectedModel)
            .Subscribe(SelectList);

        NetWorkInteraction.UpdateServerInteraction.RegisterHandler(UpdateNetWorksConfigAndWebUiNetWorkConfig);
        NetWorkInteraction.CreateServerInteraction.RegisterHandler(ReceiveAddList);
    }

    static ListViewModel()
    {
        Type type = typeof(NetWorks);
        var infos = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        s_netWorksPropInfo.AddRange(infos);
    }

    /// <summary>
    /// 将对象根据type加入到<see cref="_netWorks"/>的列表中, 并实际更新NC网络配置
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="type"></param>
    private async Task AddToNetWorkListAndUpdateWebUiNetWorkConfig(ServerObject obj, ServerType type)
    {
        if (_netWorks is null) GetWebUiNetWorkConfig();
        if (_netWorks is null) return;

        var netWorkList = GetNetWorkList(type);
        if (netWorkList is null) return;

        netWorkList.Add(obj);
        UpdateWebUiNetWorkConfig();
    }

    /// <summary>
    /// 根据<see cref="_netWorks"/>列表中的配置,更新WebUi的网络配置
    /// </summary>
    public async void UpdateWebUiNetWorkConfig()
    {
        //string api = Core.MsgHandle.Utils.JoinUrlProtAPI(CoreConfigValueAndObject.HttpUri, "6099", "/api/OB11Config/SetConfig");
        string api = ConfigValue.WebUri + Core.MsgHandle.Utils.WEBUISETCONFIG;
        System.Net.Http.HttpClient hc = new System.Net.Http.HttpClient();
        hc.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", ConfigValue.AuthToken);
        var jsonValue = GetNetWorkJson();
        if(jsonValue is null)
            return;
        
        var r = await SendMsg.PostSend(hc, api, jsonValue, null);
        var requStr = r.Content.ReadAsStringAsync();
    }

    /// <summary>
    /// 获取网络配置的Json, 从 <see cref="_netWorks"/> 中序列化
    /// </summary>
    /// <returns>如果_netWorks为 <see cref="Nullable"/> 则返回 <see cref="Nullable"/> </returns>
    public string? GetNetWorkJson()
    {
        if (_netWorks is null)
            return null;
        
        var confJsonValue = new NetWorkSetConfigValue()
        {
            EnableLocalFile2Url = _enableLocalFile2Url,
            MusicSignUrl = _musicSignUrl,
            ParseMultMsg = _parseMultMsg,
            NetWork = _netWorks
        };
        var jsonObject = new NetWorkSetConf() { Config = JsonSerializer.Serialize(confJsonValue) };
        return JsonSerializer.Serialize(jsonObject);
    }

    /// <summary>
    /// 更新<see cref="_netWorks"/> 中的网络配置
    /// </summary>
    private void UpdateNetWorksConfigAndWebUiNetWorkConfig(IInteractionContext<(ServerObject obj, ServerType type), Unit> iicontext)
    {
        var input = iicontext.Input;
        ServerType type = input.type;
        ServerObject obj = input.obj;
        iicontext.SetOutput(Unit.Default);
        
        var netWorkList = GetNetWorkList(type);
        var eqNameObj = GetExistNameNetWork(netWorkList, obj);
        if(eqNameObj is null) return;
        
        netWorkList.Remove(eqNameObj);
        netWorkList.Add(obj);
        
        UpdateWebUiNetWorkConfig();
    }

    /// <summary>
    /// 判断给定对象是否与现有配置同名，如果是的话会更改此对象的名称
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="type"></param>
    private void IsExistNameAndEdit(ServerObject obj, ServerType type)
    {
        var nameInfo = type.GetProperty("Name");
        if (nameInfo == null) return;
        
        var oldNetWork = GetExistNameNetWork(GetNetWorkList(type), obj);
        if(oldNetWork is null) return;
        
        var oldName = nameInfo.GetValue(obj) as string;
        var newName = oldName + Environment.TickCount;
        nameInfo.SetValue(obj, newName);
    }

    /// <summary>
    /// 从给定<see cref="GetNetWorkList"/>集合判断是否存在与obj同名的对象
    /// </summary>
    /// <param name="netWorkList"></param>
    /// <returns>存在返回<see cref="object"/>, 不存在返回<see cref="Nullable"/></returns>
    private object? GetExistNameNetWork(IList netWorkList, ServerObject obj)
    {
        if(netWorkList == null || netWorkList.Count == 0) return null;
        ServerType type = netWorkList[0]!.GetType();
        
        var nameInfo = type.GetProperty("Name");
        if(nameInfo is null) return null;
        var newConfigName = (string?)nameInfo.GetValue(obj);
        foreach (ServerObject o in netWorkList) { 
            var oldConfigName = (string?)nameInfo.GetValue(o);
            if(string.IsNullOrEmpty(oldConfigName) || string.IsNullOrEmpty(newConfigName))
                continue;
    
            if (oldConfigName == newConfigName) {
                return o;
            }
        }
        return null;
    }
    
    /// <summary>
    /// 从 <see cref="_netWorks"/> 中获取此类型的<see cref="IList"/>
    /// </summary>
    /// <param name="type">网络类型</param>
    /// <returns></returns>
    private IList? GetNetWorkList(ServerType type)
    {
        PropertyInfo? targetInfo = s_netWorksPropInfo.FirstOrDefault(f => f.Name.Contains(type.Name));
        if (targetInfo is null) return null;
        return (IList?)targetInfo.GetValue(_netWorks);
    }
    
    /// <summary>
    /// 从WebUI拉取网络配置
    /// </summary>
    private async void GetWebUiNetWorkConfig()
    {
        string s = await Core.MsgHandle.Utils.GetNetWorkConfig(ConfigValue.WebUri, ConfigValue.AuthToken);
        if(!s.GetJsonElement(out var netWorkJson))
            return;
        if(!netWorkJson.TryGetPropertyValue("network", out var network))
            return;

        if (netWorkJson.TryGetPropertyValue("musicSignUrl", out var musicSign))
            _musicSignUrl = musicSign.GetString() ?? "";
        if (netWorkJson.TryGetPropertyValue("enableLocalFile2Url", out var enableLocalFile2Url))
            _enableLocalFile2Url = enableLocalFile2Url.GetBoolean();
        if (netWorkJson.TryGetPropertyValue("parseMultMsg", out var parseMultMsg))
            _parseMultMsg = parseMultMsg.GetBoolean();
        
        try {
            _netWorks = network.Deserialize<NetWorks>()!;
        } catch (Exception e) {
            Loging.Log.Erro("ListViewModel::SetConfig 网络配置解析失败!", e.Message, e.StackTrace);
            return;
        }
        Add(_netWorks);
    }

    private void SelectList(NetSelectModel? model)
    {
        if (model == null) {
            foreach (var o in allNetWorkConfig) {
                if (!NetWorkConfig.Contains(o))
                    NetWorkConfig.Add(o);
            }
            return;
        }

        for (int i = 0; i < allNetWorkConfig.Count; i++) {
            var currModel = allNetWorkConfig[i];
            if (currModel.GetType() == model.ViewModelType) {
                if (!NetWorkConfig.Contains(currModel)) {
                    NetWorkConfig.Add(currModel);
                }
            } else {
                NetWorkConfig.Remove(currModel);
            }
        }
    }

    /// <summary>
    /// 将对象添加到网络配置列表<see cref="_netWorks"/>
    /// </summary>
    /// <param name="interaction"></param>
    private void ReceiveAddList(IInteractionContext<(object, ServerType), Unit> interaction)
    {
        //Handel => 发送object
        //RegionHandel => 接收object
        (object obj, ServerType type) input = interaction.Input;
        IsExistNameAndEdit(input.obj, input.type); //检查名称并更改
        Add(input.obj, input.type); //添加到显示列表
        interaction.SetOutput(Unit.Default);

        //将配置添加到与Web交互的对象中，并更新Web网络配置
        _ = AddToNetWorkListAndUpdateWebUiNetWorkConfig(input.obj, input.type);
    }
    
    private void Add(object obj) => AddAll([obj]);
    private void Add(object obj, Type type) => AddAll([obj], type.Name);
    private void AddAll<T>(List<T> list, string? typeName_ = null)
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
            Core.Utils.TypeMap(f.GetType(), viewType, f, viewModelObj);
            allNetWorkConfig.Add(viewModelObj);
            NetWorkConfig.Add(viewModelObj);
        });
    }

    private void Add(NetWorks config)
    {
        allNetWorkConfig.Clear();
        NetWorkConfig.Clear();
        AddAll(config.HttpClients);
        AddAll(config.HttpServers);
        AddAll(config.HttpSseServers);
        AddAll(config.WebSocketClients);
        AddAll(config.WebSocketServers);
    }
}