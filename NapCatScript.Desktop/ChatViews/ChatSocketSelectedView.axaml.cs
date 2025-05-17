using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NapCatScript.Desktop.ViewModels.ChatViewModels;

namespace NapCatScript.Desktop.ChatViews;

public partial class ChatSocketSelectedView : ChatView<ChatSocketSelectedView, ChatSocketSelectedViewModel>
{
    public ChatSocketSelectedView()
    {
        InitializeComponent();
    }
}