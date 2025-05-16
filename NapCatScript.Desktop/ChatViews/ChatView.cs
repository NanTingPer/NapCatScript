using Avalonia.ReactiveUI;
using NapCatScript.Desktop.ViewModels;

namespace NapCatScript.Desktop.ChatViews;

public abstract class ChatView<TSelf ,TViewModel> : ReactiveUserControl<TViewModel> 
    where TViewModel : ViewModelBase
    where TSelf : ChatView<TSelf, TViewModel>
{
    static ChatView()
    {
        ViewLocator.ViewModelMap.Add(typeof(TViewModel), typeof(TSelf));
    }
}