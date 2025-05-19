using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Text.Json;
using NapCatScript.Core.JsonFormat;
using NapCatScript.Core.NetWork.NetWorkModel;
using NapCatScript.Core.Services;
using NapCatScript.Desktop.ViewModels;
using NapCatScript.Desktop.ViewModels.NetWorkModels;
using ReactiveUI;
using static NapCatScript.Core.Utils;
using static NapCatScript.Core.MsgHandle.Utils;
using static NapCatScript.Desktop.ConfigValue;

using static NapCatScript.Desktop.ChatViews.InteractionHandler;

namespace NapCatScript.Desktop.ChatViews.ChatMain;

public class ChatSocketSelectedViewModel : ViewModelBase
{
    private bool _isSelected;
    private NetWorkSetConfigValue? _netWorkConfig;
    private object? _selectedServer;
    //private static  InteractionHandlerObject
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

    private async void YesServer()
    {
        if(SelectedServer is null) return;
        if (!_isSelected) {
            bool value = await LinkWebSocketServerInteraction
                    .Handle((WebSocketServer)((WebSocketServerViewModel)SelectedServer).GetNetWork());
            
            if(value != true)
                return;
            
            CurrentServerSelectedList.Clear();
            foreach (var o in HttpServerSelectedList) {
                CurrentServerSelectedList.Add(o);
            }
            _isSelected = !_isSelected;
            
        } else {
            bool value = await LinkHttpServerInteraction
                    .Handle((HttpServer)((HttpServerViewModel)SelectedServer).GetNetWork());
            
            if(value != true)
                return;
            
            
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