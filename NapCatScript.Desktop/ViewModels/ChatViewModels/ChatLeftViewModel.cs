using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.WebSockets;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using NapCatScript.Core.Model;
using NapCatScript.Core.MsgHandle;
using NapCatScript.Core.NetWork.NetWorkModel;
using NapCatScript.Core.Services;
using ReactiveUI;

namespace NapCatScript.Desktop.ViewModels.ChatViewModels;

public class ChatLeftViewModel : ViewModelBase
{
    public static string? CurrentHttpUri;
    public static string? CurrentWebSocketUri;
    public static Send? Send { get; private set; }
    public static ClientWebSocket? Ws { get; private set; }

    public static CancellationTokenSource CTS = new CancellationTokenSource();
    public static CancellationToken CToken => CTS.Token;
    public static SQLiteHelper<MsgInfo> SQLiteHelper { get; } = new (Path.Combine("Desktop", "MsgInfo.data"));

    private ChatSelectedMiniViewModel? _currentMiniViewModel;
    
    /// <summary>
    /// Key => 群号, ViewModel => 展示内容
    /// </summary>
    public Dictionary<long, ChatSelectedMiniViewModel> GroupViewMap { get; } = [];

    /// <summary>
    /// Key => 好友id, ViewModel => 展示内容
    /// </summary>
    public Dictionary<long, ChatSelectedMiniViewModel> PrivateViewMap { get; } = [];

    public ObservableCollection<ChatSelectedMiniViewModel> GroupViews { get; } = [];
    public ObservableCollection<ChatSelectedMiniViewModel> PrivateViews { get; } = [];

    public ChatSelectedMiniViewModel? CurrentMiniViewModel { get => _currentMiniViewModel; set => this.RaiseAndSetIfChanged(ref _currentMiniViewModel, value); }
    
    public ChatLeftViewModel()
    {
        this.WhenAnyValue(@this => @this.CurrentMiniViewModel)
            .Where(f => f != null)
            .Subscribe(f =>
            {
                InteractionHandler.NoticeRightGoToChatInteraction.Handle(f).Subscribe();
            });
        InteractionHandler.NoticeHttpServerInteraction.RegisterHandler(HttpServerInteractionHandler);
        InteractionHandler.NoticeLeftWebSocketServerInteraction.RegisterHandler(WebSocketServerInteractionHandler);
    }

    private void HttpServerInteractionHandler(IInteractionContext<HttpServer, Unit> interaction)
    {
        var input = interaction.Input;
        CurrentHttpUri = "http://" + ConfigValue.WebUriHostNotPort/*input.Host*/ + ":" + input.Port;
        interaction.SetOutput(Unit.Default);
        Send = new Send(CurrentHttpUri, input.Token);
        InitGroupList();
    }

    /// <summary>
    /// 初始化群组列表, 并创建对应的消息数据库
    /// </summary>
    private async void InitGroupList()
    {
        var list = await Send.GetGroupList();
        if(list is null) return;
        
        //创建表 初始化列表
        foreach (var groupInfo in list) {
            await SQLiteHelper.CreateTableAsync("g" + groupInfo.GroupId);

            if (GroupViewMap.TryGetValue(groupInfo.GroupId, out var f)) {
                f.GroupName = groupInfo.GroupName;
                f.GroupRemark = groupInfo.GroupRemark;
                continue;
            }
                
            
            GroupViewMap[groupInfo.GroupId] = new ChatSelectedMiniViewModel()
            {
                GroupId = groupInfo.GroupId,
                GroupName = groupInfo.GroupName,
                GroupRemark = groupInfo.GroupRemark
            };
            
            GroupViews.Add(GroupViewMap[groupInfo.GroupId]);
        }
    }
    
    private async void WebSocketServerInteractionHandler(IInteractionContext<WebSocketServer, Unit> interaction)
    {
        CTS.Cancel();
        var ws = interaction.Input;
        CurrentWebSocketUri = "ws://" + ConfigValue.WebUriHostNotPort/*ws.Host*/ + ":" + ws.Port + "/";
        interaction.SetOutput(Unit.Default);
        
        Ws = new ClientWebSocket();
        Ws.Options.SetRequestHeader("Authorization", "Bearer " + ws.Token);
        CTS = new CancellationTokenSource();
        await Ws.ConnectAsync(new Uri(CurrentWebSocketUri), CTS.Token);
        _ = Task.Run(MsgReceive);
    }

    /// <summary>
    /// 接收消息, 存入数据库, 更新GroupViewMap
    /// </summary>
    private async void MsgReceive()
    {
        while (true) {
            if(CTS.Token.IsCancellationRequested)
                break; 
            MsgInfo? info = await Ws.Receive(CTS.Token);
            if (info != null) {
                if(CurrentMiniViewModel != null)
                    await InteractionHandler.NoticeRightNewMsgInteraction.Handle(info);
                    
                _ = SQLiteHelper.InsertAsync("g" + info.GroupId, info);
                if(!long.TryParse(info.GroupId, out var groupId))
                    continue;

                if (!GroupViewMap.TryGetValue(groupId, out var view)) {
                    GroupViewMap[groupId] = new ChatSelectedMiniViewModel()
                    {
                        GroupId = groupId,
                        NewMsg = info.MessageContent
                    };
                    GroupViews.Add(GroupViewMap[groupId]);
                    continue;
                }
                
                view.NewMsg = info.MessageContent;
            }
        }
    }
}