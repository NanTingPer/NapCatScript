using System;
using System.Net.Http.Headers;
using System.Net.WebSockets;
using System.Reactive;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using NapCatScript.Core.NetWork.NetWorkModel;
using NapCatScript.Core.Services;
using ReactiveUI;

using static NapCatScript.Core.MsgHandle.ReceiveMsg;

namespace NapCatScript.Desktop.ViewModels.ChatViewModels;

public static class InteractionHandler
{
    private static bool _isLink = false;
    
    [ModuleInitializer]
    public static void Register()
    {
        LinkWebSocketServerInteraction.RegisterHandler(LinkWebSocketServerInteractionHandler);
        LinkHttpServerInteraction.RegisterHandler(LinkHttpServerInteractionHandler);
    }
    
    /// <summary>
    /// 用于连接WebSocketServer. 此连接用于接收消息
    /// <para> Handler提供者 <see cref="ChatSocketSelectedViewModel"/> </para>
    /// </summary>
    public static readonly Interaction<WebSocketServer, bool> LinkWebSocketServerInteraction = new();
    
    /// <summary>
    /// 用于连接HttpServer. 此连接用于发送消息
    /// <para> Handler提供者 <see cref="ChatSocketSelectedViewModel"/> </para>
    /// </summary>
    public static readonly Interaction<HttpServer, bool> LinkHttpServerInteraction = new();
    
    /// <summary>
    /// 如果连接正确, 通知 <see cref="ChatLeftViewModel"/>
    /// </summary>
    
    public static readonly Interaction<HttpServer, Unit> NoticeLeftHttpServerInteraction = new();
    
    /// <summary>
    /// 如果连接正确, 通知 <see cref="ChatLeftViewModel"/>
    /// </summary>
    
    public static readonly Interaction<WebSocketServer, Unit> NoticeLeftWebSocketServerInteraction = new();
    
    private static ClientWebSocket? _sSocket = null;

    private static async Task LinkHttpServerInteractionHandler(IInteractionContext<HttpServer, bool> interaction)
    {
        var input = interaction.Input;
        var tmpHc = new System.Net.Http.HttpClient();
        tmpHc.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", input.Token);
        string url = "http://" + input.Host + ":" + input.Port;
        var r = await tmpHc.GetAsync(url);

        string str = await r.Content.ReadAsStringAsync();
        /*if ((int)r.StatusCode != 200) {*/
         if(!string.IsNullOrEmpty(str)){
            interaction.SetOutput(false);
            return;
        }
        
        
        interaction.SetOutput(true);
        
        NoticeLeftHttpServerInteraction.Handle(input).Subscribe();
    }
    
    private static async Task LinkWebSocketServerInteractionHandler(IInteractionContext<WebSocketServer, bool> interaction)
    {
        if (_isLink == true) {
            interaction.SetOutput(true);
            return;
        }
        
        var ws = interaction.Input;
        _sSocket = new ClientWebSocket();
        _sSocket.Options.SetRequestHeader("Authorization", "Bearer " + ws.Token);
        
        Uri wsUri = new Uri("ws://" + ConfigValue.WebUriHostNotPort/*ws.Host*/ + ":" + ws.Port + "/");
        await _sSocket.ConnectAsync(wsUri, CancellationToken.None);
        var c= _sSocket.State;
        if(c != WebSocketState.Closed)
            interaction.SetOutput(true);
        else {
            interaction.SetOutput(false);
            _isLink = false;
            return;
        }

        NoticeLeftWebSocketServerInteraction.Handle(ws).Subscribe();
        
        await _sSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, CancellationToken.None);
        _isLink = true;
    }

}

//WebUi     terminal
//http://127.0.0.1:6099/api/Log/terminal/create  POST  200OK
//      authorization : Bearer
//      负载:
//          cols: 80
//          rows: 24
//      
//      返回:
//          {"code": 0,"data": { "id": "16e51a04-a108-4a1d-b91a-b580f10ba95a"},"message": "success"}

//如何连接: 使用WebSocket
//ws://127.0.0.1:6099/api/ws/terminal
//      负载:
//          id : create.getid
//          token : ConfigValue.Aut

//如何使用: 使用WebSocket
//发送 : ipconfig
//接收 : 回执消息

//如何关闭: 关闭控制台 
//http://127.0.0.1:6099/api/Log/terminal/id/close   id => create.getid
//      POST 200OK
//      authorization : Bearer