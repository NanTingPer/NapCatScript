using System;
using System.Collections.Generic;
using NapCatScript.Core.NetWork.NetWorkModel;
using NapCatScript.Desktop.ViewModels.NetWorkModels;

namespace NapCatScript.Desktop.Models;

public sealed class NetSelectModel
{
    public static List<NetSelectModel?> NetSelectModels { get;} =
    [
        null,
        new NetSelectModel(){ Name = "Http服务器", Type = typeof(HttpServer), EnglishName = nameof(HttpServer), ViewModelType = HttpServerViewModel.Type},
        new NetSelectModel(){ Name = "Http客户端", Type = typeof(HttpClient), EnglishName = nameof(HttpClient), ViewModelType = HttpClientViewModel.Type},
        new NetSelectModel(){ Name = "WebSocket服务器", Type = typeof(WebSocketServer), EnglishName = nameof(WebSocketServer), ViewModelType = WebSocketServerViewModel.Type},
        new NetSelectModel(){ Name = "Http SSE服务器", Type = typeof(HttpSseServer), EnglishName = nameof(HttpSseServer), ViewModelType = HttpSseServerViewModel.Type},
        new NetSelectModel(){ Name = "WebSocket客户端", Type = typeof(WebSocketClient), EnglishName = nameof(WebSocketClient), ViewModelType = WebSocketClientViewModel.Type},
    ];
    private NetSelectModel()
    {
    }

    public string Name { get; set; }
    public string EnglishName { get; set; }
    public Type Type { get; set; }
    
    public Type ViewModelType { get; set; }
    
}