using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Threading;
using ReactiveUI;

namespace NapCatScript.Desktop.ChatViews.ChatMain;

public partial class ChatRightMsgView : ChatView<ChatRightMsgView, ChatRightMsgViewModel>
{
    public ChatRightMsgView()
    {
        InitializeComponent();
        ViewModelBinding();
        /*viewModel => viewModel.time)
            .Subscribe(f =>
        {
            MsgList.ScrollToEnd();
        }*/
    }

    private async void ViewModelBinding()
    {
        while (true) {
            await Task.Delay(100);
            if (ViewModel == null) continue;
            ViewModel.ToEndInteraction.RegisterHandler(ToEndHandler);
            return;
        }
    }
    
    private void ToEndHandler(IInteractionContext<Unit, Unit> inter)
    {
        inter.SetOutput(Unit.Default);
        Dispatcher.UIThread.Post(() => { MsgList.ScrollToEnd(); });

    }
}