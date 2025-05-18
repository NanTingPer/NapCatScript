using System.Reactive;
using NapCatScript.Core.NetWork.NetWorkModel;
using ReactiveUI;

namespace NapCatScript.Desktop.ViewModels.ChatViewModels;

public class ChatRightViewModel : ViewModelBase
{
    private ViewModelBase? _chatSocketSelectedViewModel;
    private ViewModelBase _currentViewModel;
    private ViewModelBase? _chatMsgView;
    private ViewModelBase ChatMsgView => _chatMsgView ??= new ChatRightMsgViewModel();
    private ViewModelBase ChatSocketSelectedViewModel => _chatSocketSelectedViewModel ??= new ChatSocketSelectedViewModel();

    public ViewModelBase CurrentViewModel { get => _currentViewModel; set => this.RaiseAndSetIfChanged(ref _currentViewModel, value); }
     
    public ChatRightViewModel()
    {
        CurrentViewModel = ChatSocketSelectedViewModel;
        InteractionHandler.NoticeHttpServerInteraction.RegisterHandler(GotoView);
    }

    private void GotoView(IInteractionContext<HttpServer, Unit> interaction)
    {
        CurrentViewModel = ChatMsgView;
    }
}