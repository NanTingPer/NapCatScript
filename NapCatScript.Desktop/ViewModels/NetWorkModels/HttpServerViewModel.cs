using System;
using System.Threading.Tasks;
using NapCatScript.Core;
using NapCatScript.Core.NetWork.NetWorkModel;
using ReactiveUI;

namespace NapCatScript.Desktop.ViewModels.NetWorkModels;

public class HttpServerViewModel : ViewModelBase
{
    private Type httpServerType = typeof(HttpServer);
    private Type thisType = typeof(HttpServerViewModel);
    private string _name = "HttpServer";
    private bool _enable = false;
    private int _port = 9998;
    private string _host = "127.0.0.1";
    private bool _enableCors = false;
    private bool _enableWebSocket = false;
    private string _messagePostFormat = "string";
    private string _token = "";
    private bool _debug = false;
    
    public IReactiveCommand AddNetWorkCommand { get; set; }

    public string Name { get => _name; set => this.RaiseAndSetIfChanged(ref _name, value); } 
    public bool Enable { get => _enable; set => this.RaiseAndSetIfChanged(ref _enable, value); }
    public int Port { get => _port; set => this.RaiseAndSetIfChanged(ref _port, value); }
    public string Host { get => _host; set => this.RaiseAndSetIfChanged(ref _host, value); }
    public bool EnableCors { get => _enableCors; set => this.RaiseAndSetIfChanged(ref _enableCors, value); }
    public bool EnableWebSocket { get => _enableWebSocket; set => this.RaiseAndSetIfChanged(ref _enableWebSocket, value); }
    public string MessagePostFormat { get => _messagePostFormat; set => this.RaiseAndSetIfChanged(ref _messagePostFormat, value); }
    public string Token { get => _token; set => this.RaiseAndSetIfChanged(ref _token, value); }
    public bool Debug { get => _debug; set => this.RaiseAndSetIfChanged(ref _debug, value); }

    public HttpServerViewModel()
    {
        AddNetWorkCommand = ReactiveCommand.Create(CreateNetWork);
    }
    public HttpServerViewModel(HttpServer httpServer) : this()
    {
        Utils.TypeMap(thisType, httpServerType, this, httpServer);
    }

    public void CreateNetWork()
    {
        HttpServer config = new HttpServer();
        Type type = config.GetType();
        Utils.TypeMap(this.GetType(), type, this, config);
        NetWorkInteraction.CreateServerInteraction.Handle((config, type)).Subscribe();
    }
}