using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NapCatScript.Desktop.ChatViews.ChatMain;

namespace NapCatScript.Desktop.ChatViews;

public partial class ChatMsgView : ChatView<ChatMsgView, ChatMsgViewModel>
{

    public ChatMsgView()
    {
        InitializeComponent();
    }
}