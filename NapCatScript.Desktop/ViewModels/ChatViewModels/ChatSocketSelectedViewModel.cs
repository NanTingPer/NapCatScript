using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Text.Json;
using NapCatScript.Core.JsonFormat;
using NapCatScript.Core.NetWork.NetWorkModel;
using NapCatScript.Core.Services;
using NapCatScript.Desktop.ViewModels.NetWorkModels;
using ReactiveUI;

using static NapCatScript.Core.Utils;
using static NapCatScript.Core.MsgHandle.Utils;
using static NapCatScript.Desktop.ConfigValue;

namespace NapCatScript.Desktop.ViewModels.ChatViewModels;

public class ChatSocketSelectedViewModel : ViewModelBase
{
    /// <summary>
    /// 用于连接WebSocketServer. 此连接用于接收消息
    /// </summary>
    public static readonly Interaction<WebSocketServer, bool> LinkWebSocketServerInteraction = new();
    
    /// <summary>
    /// 用于连接HttpServer. 此连接用于发送消息
    /// </summary>
    public static readonly Interaction<HttpServer, bool> LinkHttpServerInteraction = new();

    private NetWorkSetConfigValue? _netWorkConfig;
    private object? _selectedServer;

    public ObservableCollection<object> CurrentServerSelectedList { get; } = [];
    public ObservableCollection<object> WebSocketServerSelectedList { get; } = [];
    public ObservableCollection<object> HttpServerSelectedList { get; } = [];
    public object? SelectedServer { get => _selectedServer; set => this.RaiseAndSetIfChanged(ref _selectedServer, value); }
    public IReactiveCommand YesServerCommand { get; set; }

    public ChatSocketSelectedViewModel()
    {
        InitNetWorkConfig();
        YesServerCommand = ReactiveCommand.Create(YesServer);
    }

    private void YesServer()
    {
        CurrentServerSelectedList.Clear();
        foreach (var o in HttpServerSelectedList) {
            CurrentServerSelectedList.Add(o);
        }
    }
    
    private async void InitNetWorkConfig()
    {
        string? configString = await GetWebUiNetWorkConfig(WebUri, AuthToken);
        try {
            configString.GetJsonElement(out var netWorkConfig);
            netWorkConfig.TryGetPropertyValue("data", out var netWorkData);
            _netWorkConfig = netWorkData.Deserialize<NetWorkSetConfigValue>();
        } catch (Exception ex) {
            Log.InstanceLog.Erro(nameof(ChatSocketSelectedViewModel) + "无法获取网络配置",ex.ToString(), ex.Message);
            return;
        }
        
        
        foreach (var netWorkHttpServer in _netWorkConfig!.NetWork.HttpServers) {
            var hsvm = new HttpServerViewModel();
            TypeMap(netWorkHttpServer.GetType(), hsvm.GetType(), netWorkHttpServer, hsvm);
            HttpServerSelectedList.Add(hsvm);
        }
        foreach (var netWorkWebSocketServer in _netWorkConfig!.NetWork.WebSocketServers) {
            var wsswm = new WebSocketServerViewModel();
            TypeMap(netWorkWebSocketServer.GetType(), wsswm.GetType(), netWorkWebSocketServer, wsswm);
            WebSocketServerSelectedList.Add(wsswm);
        }
        foreach (var o in WebSocketServerSelectedList) {
            CurrentServerSelectedList.Add(o);
        }
    }
}