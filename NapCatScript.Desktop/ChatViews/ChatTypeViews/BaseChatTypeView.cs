using System;
using Avalonia.ReactiveUI;
using NapCatScript.Desktop.ViewModels;

namespace NapCatScript.Desktop.ChatViews.ChatTypeViews;

public abstract class BaseChatTypeView<TSelf ,TViewModel> : ReactiveUserControl<TViewModel> 
    where TViewModel : ViewModelBase
    where TSelf : BaseChatTypeView<TSelf, TViewModel>
{
    static BaseChatTypeView()
    {
        Type viewModelType = typeof(TViewModel);
        if (!ViewLocator.ViewModelMap.TryGetValue(viewModelType, out _)) {
            ViewLocator.ViewModelMap.Add(viewModelType, typeof(TSelf));
        }
    }
}