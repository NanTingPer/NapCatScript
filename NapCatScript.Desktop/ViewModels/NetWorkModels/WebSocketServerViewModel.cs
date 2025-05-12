using NapCatScript.Core.NetWork.NetWorkModel;
using ReactiveUI;

namespace NapCatScript.Desktop.ViewModels.NetWorkModels;

public class WebSocketServerViewModel : ConfigModel<WebSocketServerViewModel, WebSocketServer>
{
    private string _name = "WebSocketServer";
    private bool _enable = false;
    private string _host = "127.0.0.1";
    private int _port = 9999;
    private string _messagePostFormat = "string";
    private bool _reportSelfMessage = false;
    private string _token = "";
    private bool _enableForcePushEvent = false;
    private bool _debug = false;
    private int _heartInterval = 50000;
    
    public string Name { get => _name; set => this.RaiseAndSetIfChanged(ref _name, value); }
    public bool Enable { get => _enable; set => this.RaiseAndSetIfChanged(ref _enable, value); }
    public string Host { get => _host; set => this.RaiseAndSetIfChanged(ref _host, value); }
    public int Port { get => _port; set => this.RaiseAndSetIfChanged(ref _port, value); }
    public string MessagePostFormat { get => _messagePostFormat; set => this.RaiseAndSetIfChanged(ref _messagePostFormat, value); }
    public bool ReportSelfMessage { get => _reportSelfMessage; set => this.RaiseAndSetIfChanged(ref _reportSelfMessage, value); }
    public string Token { get => _token; set => this.RaiseAndSetIfChanged(ref _token, value); }
    public bool EnableForcePushEvent { get => _enableForcePushEvent; set => this.RaiseAndSetIfChanged(ref _enableForcePushEvent, value); }
    public bool Debug { get => _debug; set => this.RaiseAndSetIfChanged(ref _debug, value); }
    public int HeartInterval { get => _heartInterval; set => this.RaiseAndSetIfChanged(ref _heartInterval, value); }
}