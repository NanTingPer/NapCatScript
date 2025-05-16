using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NapCatScript.Desktop.ViewModels.ChatViewModels;

namespace NapCatScript.Desktop.ChatViews;

public partial class ChatMainView : ChatView<ChatMainView, ChatMainViewModel>
{
    private ChatLeftViewModel? _chatLeftViewModel;
    private ChatRightViewModel? _chatRightViewModel;
    public ChatLeftViewModel LeftViewModel => _chatLeftViewModel ??= new ChatLeftViewModel();
    public ChatRightViewModel RightViewModel => _chatRightViewModel ??= new ChatRightViewModel();
    public ChatMainView()
    {
        InitializeComponent();
    }
}