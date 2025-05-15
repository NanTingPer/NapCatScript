using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls.Shapes;
using DynamicData;
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
    public ObservableCollection<WebUiLoginInfo> WebUiLoginInfos { get; set; } = [];
    
    public IReactiveCommand LoginCommand { get; set; }

    public WebUIConnectionViewModel()
    {
        LoginCommand = ReactiveCommand.Create(Login);
        Interactions.DeleteLoginInfoInteraction.RegisterHandler(DeleteLoginInfo);
        Interactions.LoginWebUiInteraction.RegisterHandler(LoginHandler);
        /*#if DEBUG
        Url = "http://127.0.0.1:6099";
        Token = "napcat";
        #endif*/
        InitLoginInfo();
    }

    private void LoginHandler(IInteractionContext<WebUiLoginInfo, Unit> context)
    {
        var o = context.Input;
        context.SetOutput(Unit.Default);
        Login(o.Url, o.Token);
    }
    
    private async void DeleteLoginInfo(IInteractionContext<WebUiLoginInfo, Unit> inter)
    {
        var input = inter.Input;
        inter.SetOutput(Unit.Default);
        await SQLiteServer.Delete<WebUiLoginInfo>(input.Url);
        for (var i = 0; i < WebUiLoginInfos.Count; i++) {
            var obj = WebUiLoginInfos[i];
            if (obj.Url != input.Url) continue;
            WebUiLoginInfos.Remove(obj);
            break;
        }
    }
    
    private async void InitLoginInfo()
    {
        var list = await SQLiteServer.GetAll<WebUiLoginInfo>();
        var loginInfo = list.FirstOrDefault(f => true);
        if(loginInfo == null) return;
        
        Url = loginInfo.Url;
        Token = loginInfo.Token;
        foreach (var webUiLoginInfo in list) {
            WebUiLoginInfos.Add(webUiLoginInfo);
        }
    }
    
    private void Login(string url, string token)
    {
        Interactions.WebUIConnectionInteraction
            .Handle((url, token))
            .Subscribe(f => { if(f) AddLoginInfo(); });
    }

    private void Login() => Login(Url, Token);
    
    private async void AddLoginInfo()
    {
        var insert = new WebUiLoginInfo{Url = Url, Token = Token};
        await SQLiteServer.Insert(insert, "Url");
    }
}

public class WebUiLoginInfo : ViewModelBase
{
    [SQLite.PrimaryKey]
    public string Url { get; set; }
    public string Token { get; set; }

    [SQLite.Ignore]
    public IReactiveCommand DeleteInfoCommand { get; set; }

    [SQLite.Ignore]
    public IReactiveCommand LoginCommand {get; set; }
    
    public WebUiLoginInfo()
    {
        DeleteInfoCommand = ReactiveCommand.Create(DeleteInfo);
        LoginCommand = ReactiveCommand.Create(Login);
    }
    
    private void DeleteInfo()
    {
        Interactions.DeleteLoginInfoInteraction.Handle(this).Subscribe();
    }

    private void Login()
    {
        Interactions.LoginWebUiInteraction.Handle(this).Subscribe();
    }
}