using System;
using System.Collections.Generic;
using NapCatScript.Core.NetWork.NetWorkModel;

namespace NapCatScript.Desktop.Models;

public sealed class NetSelectModel
{
    public static List<NetSelectModel> NetSelectModels { get; set; } =
    [
        new NetSelectModel(){ Name = "Http服务器", Type = typeof(HttpServer), EnglishName = nameof(HttpServer)},
        new NetSelectModel(){ Name = "Http客户端", Type = typeof(HttpClient), EnglishName = nameof(HttpClient)},
        new NetSelectModel(){ Name = "WebSocket服务器", Type = typeof(WebSocketServer), EnglishName = nameof(WebSocketServer)},
        new NetSelectModel(){ Name = "Http SSE服务器", Type = typeof(HttpSseServer), EnglishName = nameof(HttpSseServer)},
        new NetSelectModel(){ Name = "WebSocket客户端", Type = typeof(WebSocketClient), EnglishName = nameof(WebSocketClient)},
    ];
    private NetSelectModel()
    {
    }

    public string Name { get; set; }
    public string EnglishName { get; set; }
    public Type Type { get; set; }
    
    
}