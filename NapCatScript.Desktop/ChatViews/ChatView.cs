using System;
using System.Runtime.CompilerServices;
using Avalonia.ReactiveUI;
using NapCatScript.Desktop.ViewModels;
namespace NapCatScript.Desktop.ChatViews;

public abstract class ChatView<TSelf ,TViewModel> : ReactiveUserControl<TViewModel> 
    where TViewModel : ViewModelBase
    where TSelf : ChatView<TSelf, TViewModel>
{
    static ChatView()
    {
        Type viewModelType = typeof(TViewModel);
        if (!ViewLocator.ViewModelMap.TryGetValue(viewModelType, out _)) {
            ViewLocator.ViewModelMap.Add(viewModelType, typeof(TSelf));
        }
    }
}