using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls.Shapes;
using NapCatScript.Core.Services;
using ReactiveUI;
using Console = System.Console;
using Path = System.IO.Path;

namespace NapCatScript.Desktop.ViewModels;

public class WebUIConnectionViewModel : ViewModelBase
{
    private string _url = "";
    private string _token = "";
    private SQLiteService? _sqlServer;
    public string Url { get => _url; set => this.RaiseAndSetIfChanged(ref _url, value); }
    public string Token { get => _token; set => this.RaiseAndSetIfChanged(ref _token, value); }
    public SQLiteService SQLiteServer { get => _sqlServer ??= new SQLiteService(Path.Combine("Desktop", "login.data")); }
    public IReactiveCommand LoginCommand { get; set; }

    public WebUIConnectionViewModel()
    {
        LoginCommand = ReactiveCommand.Create(Login);
        /*#if DEBUG
        Url = "http://127.0.0.1:6099";
        Token = "napcat";
        #endif*/
        InitLoginInfo();
    }

    private async void InitLoginInfo()
    {
        var list = await SQLiteServer.GetAll<WebUiLoginInfo>();
        var loginInfo = list.FirstOrDefault(f => true);
        if(loginInfo == null) return;
        
        Url = loginInfo.Url;
        Token = loginInfo.Token;
    }
    
    private void Login()
    {
        Interactions.WebUIConnectionEvent
            .Handle((Url, Token))
            .Subscribe(f => { if(f) AddLoginInfo(); });
    }

    private async void AddLoginInfo()
    {
        var insert = new WebUiLoginInfo{Url = Url, Token = Token};
        await SQLiteServer.Insert(insert);
    }
}

public class WebUiLoginInfo
{
    [SQLite.PrimaryKey]
    public string Key { get => Url + ":" + Token; }
    public string Url { get; set; }
    public string Token { get; set; }
}