using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using Avalonia.Threading;
using NapCatScript.Core.JsonFormat;
using NapCatScript.Core.JsonFormat.Msgs;
using NapCatScript.Core.Model;
using ReactiveUI;

namespace NapCatScript.Desktop.ViewModels.ChatViewModels;

public class ChatRightMsgViewModel : ViewModelBase
{
    public Interaction<Unit, Unit> ToEndInteraction { get; } = new();
    
    private bool _existsSelect = false;
    private string _textMsg = "";
    public bool ExistsSelect { get => _existsSelect; set => this.RaiseAndSetIfChanged(ref _existsSelect, value); }
    public string TextMsg { get => _textMsg; set => this.RaiseAndSetIfChanged(ref _textMsg, value); }
    
    private string currentGroupId = "";
    public ObservableCollection<ChatMsgViewModel> Msgs { get; set; } = [];
    public long time = 0L;
    
    public IReactiveCommand SendMsgCommand { get; set; }
    
    public ChatRightMsgViewModel()
    {
        SendMsgCommand = ReactiveCommand.Create(SendMsg);
        InteractionHandler.NoticeRightGoToChatInteraction.RegisterHandler(GotoChatHandler);
        InteractionHandler.NoticeRightNewMsgInteraction.RegisterHandler(UpdateMsgList);
    }

    private void SendMsg()
    {
        ChatLeftViewModel.Send!.SendMsg(
            currentGroupId, 
            MsgTo.group, 
            new TextJson(TextMsg));
    }
    
    private void UpdateMsgList(IInteractionContext<MsgInfo, Unit> interaction)
    {
        var msgInfo = interaction.Input;
        interaction.SetOutput(Unit.Default);
        if(!msgInfo.GroupId.Equals(currentGroupId))
            return;
        
        Msgs.Add(new ChatMsgViewModel()
        {
            MsgContent = msgInfo.MessageContent,
            MsgId = msgInfo.MessageId.ToString(),
            NikeId = msgInfo.UserId,
            NikeName = msgInfo.UserName
        });
        time = DateTime.Now.Ticks;
        
        ToEndInteraction.Handle(Unit.Default).Subscribe();
    }
    
    private async void GotoChatHandler(IInteractionContext<ChatSelectedMiniViewModel, Unit> inter)
    {
        var tarGetChat = inter.Input;
        inter.SetOutput(Unit.Default);
        currentGroupId = tarGetChat.GroupId.ToString();
        ExistsSelect = true;
        Msgs.Clear();
        var tableName = "g" + tarGetChat.GroupId;
        var msgInfos = await ChatLeftViewModel.SQLiteHelper.QueryAsync(tableName, $"select * from {tableName}");
        msgInfos.Sort((up, down) =>
        {
            if(up.Time > down.Time)
                return 1;
            return up.Time < down.Time ? -1 : 0;
        });
        foreach (var msgInfo in msgInfos) {
            Msgs.Add(new ChatMsgViewModel()
            {
                MsgContent = msgInfo.MessageContent,
                MsgId = msgInfo.MessageId.ToString(),
                NikeId = msgInfo.UserId,
                NikeName = msgInfo.UserName
            }); 
        }
    }
}