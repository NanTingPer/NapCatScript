using System;
using System.IO;
using System.Net.WebSockets;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using NapCatScript.Core.Model;
using NapCatScript.Core.MsgHandle;
using NapCatScript.Core.NetWork.NetWorkModel;
using NapCatScript.Core.Services;
using ReactiveUI;

namespace NapCatScript.Desktop.ViewModels.ChatViewModels;

public class ChatLeftViewModel : ViewModelBase
{
    public static string? CurrentHttpUri;
    public static string? CurrentWebSocketUri;
    public static Send? Send { get; private set; }
    public static ClientWebSocket? Ws { get; private set; }

    public static CancellationTokenSource CTS = new CancellationTokenSource();
    
    public static CancellationToken CToken => CTS.Token;
    
    public ChatLeftViewModel()
    {
        InteractionHandler.NoticeLeftHttpServerInteraction.RegisterHandler(HttpServerInteractionHandler);
        InteractionHandler.NoticeLeftWebSocketServerInteraction.RegisterHandler(WebSocketServerInteractionHandler);
    }

    private async void HttpServerInteractionHandler(IInteractionContext<HttpServer, Unit> interaction)
    {
        var input = interaction.Input;
        CurrentHttpUri = "http://" + ConfigValue.WebUriHostNotPort/*input.Host*/ + ":" + input.Port;
        interaction.SetOutput(Unit.Default);
        Send = new Send(CurrentHttpUri, input.Token);
        var list = await Send.GetGroupList();
    }
    
    private void WebSocketServerInteractionHandler(IInteractionContext<WebSocketServer, Unit> interaction)
    {
        CTS.Cancel();
        var ws = interaction.Input;
        CurrentWebSocketUri = "ws://" + ConfigValue.WebUriHostNotPort/*ws.Host*/ + ":" + ws.Port + "/";
        interaction.SetOutput(Unit.Default);
        
        Ws = new ClientWebSocket();
        Ws.Options.SetRequestHeader("Authorization", "Bearer " + ws.Token);
        CTS = new CancellationTokenSource();
        _ = Task.Run(async () =>
        {
            while (true) {
                if(CTS.Token.IsCancellationRequested)
                    return; 
                MsgInfo? info = await Ws.Receive(CTS.Token);
            }
        });


    }
}