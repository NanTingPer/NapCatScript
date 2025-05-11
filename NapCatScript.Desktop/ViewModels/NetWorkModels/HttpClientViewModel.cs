using System.Collections.ObjectModel;
using NapCatScript.Core.NetWork.NetWorkModel;
using ReactiveUI;
using System.Text.Json.Serialization;

namespace NapCatScript.Desktop.ViewModels.NetWorkModels;

public class HttpClientViewModel : ConfigModel<HttpClientViewModel, NapCatScript.Core.NetWork.NetWorkModel.HttpClient>
{
    private string _name;
    private bool _enable;
    private string _url;
    private string _messagePostFormat;
    private bool _reportSelfMessage;
    private string _token;
    private bool _debug;
    public string Name { get => _name; set => this.RaiseAndSetIfChanged(ref _name, value); }
    public bool Enable { get => _enable; set => this.RaiseAndSetIfChanged(ref _enable, value); }
    public string Url { get => _url; set => this.RaiseAndSetIfChanged(ref _url, value); }
    public string MessagePostFormat { get => _messagePostFormat; set => this.RaiseAndSetIfChanged(ref _messagePostFormat, value); }
    public bool ReportSelfMessage { get => _reportSelfMessage; set => this.RaiseAndSetIfChanged(ref _reportSelfMessage, value); }
    public string Token { get => _token; set => this.RaiseAndSetIfChanged(ref _token, value); }
    public bool Debug { get => _debug; set => this.RaiseAndSetIfChanged(ref _debug, value); }
}
