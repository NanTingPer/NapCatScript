using System;
using System.Reactive;
using ReactiveUI;
using Console = System.Console;

namespace NapCatScript.Desktop.ViewModels;

public class WebUIConnectionViewModel : ViewModelBase
{
    private string _url = "";
    private string _token = "";
    public string Url { get => _url; set => this.RaiseAndSetIfChanged(ref _url, value); }
    public string Token { get => _token; set => this.RaiseAndSetIfChanged(ref _token, value); }

    public IReactiveCommand LoginCommand { get; set; }

    public WebUIConnectionViewModel()
    {
        LoginCommand = ReactiveCommand.Create(Login);
        #if DEBUG
        Url = "http://127.0.0.1:6099";
        Token = "napcat";
        #endif
    }
    
    private void Login()
    {
        Interactions.WebUIConnectionEvent.Handle((Url, Token)).Subscribe();
    }
}