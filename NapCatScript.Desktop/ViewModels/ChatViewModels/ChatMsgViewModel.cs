using ReactiveUI;

namespace NapCatScript.Desktop.ViewModels.ChatViewModels;

public class ChatMsgViewModel : ViewModelBase
{
    private string _nikeName;
    private string _nikeId;
    private string _msgContent;
    private string _msgId;

    public string NikeName { get => _nikeName; set => this.RaiseAndSetIfChanged(ref _nikeName, value); }
    public string MsgContent { get => _msgContent; set => this.RaiseAndSetIfChanged(ref _msgContent, value); }
    public string NikeId { get => _nikeId; set => this.RaiseAndSetIfChanged(ref _nikeId, value); }
    public string MsgId { get => _msgId; set => this.RaiseAndSetIfChanged(ref _msgId, value); }
}