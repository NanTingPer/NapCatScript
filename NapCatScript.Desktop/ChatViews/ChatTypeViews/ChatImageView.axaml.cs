using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace NapCatScript.Desktop.ChatViews.ChatTypeViews;

public partial class ChatImageView : BaseChatTypeView<ChatImageView, ChatImageViewModel>
{
    public ChatImageView()
    {
        InitializeComponent();
    }
}