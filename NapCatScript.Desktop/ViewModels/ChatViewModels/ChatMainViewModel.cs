using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using NapCatScript.Desktop.ChatViews;

namespace NapCatScript.Desktop.ViewModels.ChatViewModels;

public class ChatMainViewModel : ViewModelBase
{
    static ChatMainViewModel()
    {
        IEnumerable<Type?> chatViewTypes = Assembly
            .GetExecutingAssembly()
            .GetTypes()
            .Where(f => f.BaseType is not null)
            .Where(f => f.BaseType!.FullName is not null)
            .Where(f => f.BaseType!.IsGenericType)
            .Where(f => f.BaseType!.GetGenericTypeDefinition() == typeof(ChatView<,>));

        
        
        foreach (var chatViewType in chatViewTypes) {
            if(chatViewType is null) continue;
            //Type[] viewModelViewMap = chatViewType.BaseType!.GenericTypeArguments;
            //Type viewModelType = viewModelViewMap[1];
            //Type viewType = viewModelViewMap[0];
            //if (!ViewLocator.ViewModelMap.TryGetValue(viewModelType, out _)) {
            //    ViewLocator.ViewModelMap.Add(viewModelType, viewType);
            //}
            RuntimeHelpers.RunClassConstructor(chatViewType.BaseType!.TypeHandle);
        }
    }

    private ChatLeftViewModel? _chatLeftViewModel;
    private ChatRightViewModel? _chatRightViewModel;
    public ChatLeftViewModel LeftViewModel => _chatLeftViewModel ??= new ChatLeftViewModel();
    public ChatRightViewModel RightViewModel => _chatRightViewModel ??= new ChatRightViewModel();
}