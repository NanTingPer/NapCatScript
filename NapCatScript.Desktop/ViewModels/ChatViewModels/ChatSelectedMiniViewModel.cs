using System;
using ReactiveUI;

namespace NapCatScript.Desktop.ViewModels.ChatViewModels;

public class ChatSelectedMiniViewModel : ViewModelBase
{
    private double _groupId;
    private string _groupName;
    private string _groupRemark;
    private string _newMsg;
    private string _newMsgTime;
    
    public double GroupId { get => _groupId; set => this.RaiseAndSetIfChanged(ref _groupId, value); }
    public string GroupName { get => _groupName; set => this.RaiseAndSetIfChanged(ref _groupName, value); }

    public string GroupRemark
    {
        get
        {
            if(!string.IsNullOrEmpty(_groupRemark))
                return _groupRemark;
            return _groupName;
        } set => this.RaiseAndSetIfChanged(ref _groupRemark, value);
    }

    public string NewMsg
    {
        get => _newMsg;
        set
        {
            this.RaiseAndSetIfChanged(ref _newMsg, value);
            NewMsgTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
    public string NewMsgTime { get => _newMsgTime; set => this.RaiseAndSetIfChanged(ref _newMsgTime, value); }
}