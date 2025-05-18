using System.Collections.ObjectModel;
using System.IO;
using System.Reactive;
using NapCatScript.Core.Model;
using ReactiveUI;

namespace NapCatScript.Desktop.ViewModels.ChatViewModels;

public class ChatRightMsgViewModel : ViewModelBase
{
    private bool _existsSelect = false;
    public bool ExistsSelect { get => _existsSelect; set => this.RaiseAndSetIfChanged(ref _existsSelect, value); }

    private string currentGroupId = "";
    public ObservableCollection<ChatMsgViewModel> Msgs { get; set; } = [];
    
    public ChatRightMsgViewModel()
    {
        InteractionHandler.NoticeRightGoToChatInteraction.RegisterHandler(GotoChatHandler);
        InteractionHandler.NoticeRightNewMsgInteraction.RegisterHandler(UpdateMsgList);
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
                return -1;
            return up.Time < down.Time ? 1 : 0;
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