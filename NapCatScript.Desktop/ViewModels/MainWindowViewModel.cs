﻿using System;
using System.Collections.Generic;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Reactive;
using System.Threading.Tasks;
using NapCatScript.Core.JsonFormat;
using NapCatScript.Core.Model;
using NapCatScript.Core.Services;
using NapCatScript.Desktop.ChatViews;
using NapCatScript.Desktop.ChatViews.ChatMain;
using static NapCatScript.Core.MsgHandle.Utils;

namespace NapCatScript.Desktop.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private bool _webUiInit = false;
    private ViewModelBase currView;
    private string _selectedItem;

    public IReactiveCommand ListBoxPropertyChangedCommand { get; private set; }
    private ViewModelBase? _logViewModel;
    private ViewModelBase? _netWorkModel;
    private ViewModelBase? _webUiConnectionViewModel;
    private ViewModelBase? _chatViewModel;
    public ViewModelBase LogViewModel => _logViewModel ??= new LogViewModel();
    public ViewModelBase NetWorkModel => _netWorkModel ??= new NetWorkViewModel();
    public ViewModelBase WebUiConnectionViewModel => _webUiConnectionViewModel ??= new WebUIConnectionViewModel();
    public ViewModelBase ChatViewModel => _chatViewModel ??= new ChatMainViewModel();
    
    
    public ObservableCollection<string> Items { get; } = [];

    public string SelectedItem
    {
        get => _selectedItem;
        set => this.RaiseAndSetIfChanged(ref _selectedItem, value);
    }

    public ViewModelBase CurrView
    {
        get => currView;
        set => this.RaiseAndSetIfChanged(ref currView, value);
    }
    
    public MainWindowViewModel()
    {
        this.WhenAnyValue(f => f.SelectedItem)
            .WhereNotNull()
            .Subscribe(ListBoxPropertyChanged);

        Interactions.WebUIConnectionInteraction.RegisterHandler(WebUILoginEvent);
        Items.Add("连接");
        Items.Add("聊天");
        Items.Add("日志");
        Items.Add("网络");
        CurrView = WebUiConnectionViewModel;
    }

    private void ListBoxPropertyChanged(string str)
    {
        if (_webUiInit == false)
            return;
        if (str == "网络") CurrView = NetWorkModel;
        if (str == "日志") CurrView = LogViewModel;
        if (str == "连接") CurrView = WebUiConnectionViewModel;
        if (str == "聊天") CurrView = ChatViewModel;
    }

    private async Task WebUILoginEvent(IInteractionContext<(string, string), bool> interaction)
    {
        (string webURL, string token) connectionValue = interaction.Input;
        if (connectionValue.token == ConfigValue.WebToken && connectionValue.webURL == ConfigValue.WebUri) {
            interaction.SetOutput(false);
            return;
        }
        
        var rms = await WebUILogin(connectionValue.webURL, connectionValue.token);
        var requJson = await rms.Content.ReadAsStringAsync();
        if (!requJson.GetJsonElement(out var json) ||
            !json.TryGetPropertyValue("message", out var Jvalue) ||
            Jvalue.GetString() != "success") {
            interaction.SetOutput(false);
            return;
        }
            
        var aut = await GetAuthentication(connectionValue.webURL, connectionValue.token);
        ConfigValue.WebToken = connectionValue.token;
        ConfigValue.AuthToken = aut;
        ConfigValue.WebUri = connectionValue.webURL;
        interaction.SetOutput(true);
        CurrView = LogViewModel;
        _webUiInit = true;
    }
}
