using NapCatScript.Core.NetWork.NetWorkModel;
using ReactiveUI;

namespace NapCatScript.Desktop.ViewModels.NetWorkModels;

public class WebSocketClientViewModel : ConfigModel<WebSocketClientViewModel, WebSocketClient>
{
    private string _name = "WebSocketClient";
    private bool _enabled = false;
    private string _url = "ws://127.0.0.1:8082";
    private string _messagePostFormat = "string";
    private bool _reportSelfMessage = false;
    private int _reconnectInterval = 30000;
    private string _token = "";
    private bool _debug = false;
    private int _heartInterval = 30000;
    
    public string Name { get => _name; set => this.RaiseAndSetIfChanged(ref _name, value); }
    public bool Enable { get => _enabled; set => this.RaiseAndSetIfChanged(ref _enabled, value); }
    public string Url { get => _url; set => this.RaiseAndSetIfChanged(ref _url, value); }
    public string MessagePostFormat { get => _messagePostFormat; set => this.RaiseAndSetIfChanged(ref _messagePostFormat, value); }
    public bool ReportSelfMessage { get => _reportSelfMessage; set => this.RaiseAndSetIfChanged(ref _reportSelfMessage, value); }
    public int ReconnectInterval { get => _reconnectInterval; set => this.RaiseAndSetIfChanged(ref _reconnectInterval, value); }
    public string Token { get => _token; set => this.RaiseAndSetIfChanged(ref _token, value); }
    public bool Debug { get => _debug; set => this.RaiseAndSetIfChanged(ref _debug, value); }
    public int HeartInterval { get => _heartInterval; set => this.RaiseAndSetIfChanged(ref _heartInterval, value); }
}