using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NapCatScript.Desktop.ChatViews.ChatMain;

namespace NapCatScript.Desktop.ChatViews;

public partial class ChatLeftView : ChatView<ChatLeftView, ChatLeftViewModel>
{
    public ChatLeftView()
    {
        InitializeComponent();
    }
}