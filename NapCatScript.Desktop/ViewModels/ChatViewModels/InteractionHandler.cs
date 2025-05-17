using System;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using NapCatScript.Core.NetWork.NetWorkModel;
using ReactiveUI;

using static NapCatScript.Core.MsgHandle.ReceiveMsg;

namespace NapCatScript.Desktop.ViewModels.ChatViewModels;

public static class InteractionHandler
{
    [ModuleInitializer]
    public static void Register()
    {
        LinkWebSocketServerInteraction.RegisterHandler(LinkWebSocketServerInteractionHander);
    }
    
    /// <summary>
    /// 用于连接WebSocketServer. 此连接用于接收消息
    /// <para> Handel <see cref="ChatLeftViewModel"/> </para>
    /// </summary>
    public static readonly Interaction<WebSocketServer, bool> LinkWebSocketServerInteraction = new();
    
    /// <summary>
    /// 用于连接HttpServer. 此连接用于发送消息
    /// <para> Handel <see cref="ChatLeftViewModel"/> </para>
    /// </summary>
    public static readonly Interaction<HttpServer, bool> LinkHttpServerInteraction = new();
    
    private static ClientWebSocket? s_socket = null;
    private static async Task LinkWebSocketServerInteractionHander(IInteractionContext<WebSocketServer, bool> interaction)
    {
        var ws = interaction.Input;
        s_socket = new ClientWebSocket();
        s_socket.Options.SetRequestHeader("Authorization", "Bearer " + ws.Token);
        
        Uri wsUri = new Uri("ws://" + ConfigValue.WebUriHostNotPort/*ws.Host*/ + ":" + ws.Port + "/");
        await s_socket.ConnectAsync(wsUri, CancellationToken.None);
        var c= s_socket.State;
        if(c != WebSocketState.Closed)
            interaction.SetOutput(true);
        else {
            interaction.SetOutput(false);
            return;
        }

        _ = Task.Run(async () =>
        {
            while (true) {
                var cb = await s_socket.Receive(CancellationToken.None);
            }
        });
    }

}