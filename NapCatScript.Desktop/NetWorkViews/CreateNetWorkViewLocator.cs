using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using NapCatScript.Desktop.NetWorkViews.CreateViews;
using NapCatScript.Desktop.ViewModels;
using NapCatScript.Desktop.ViewModels.NetWorkModels;
using NapCatScript.Desktop.Views.NetWorkViews;
using ViewType = System.Type;
using ViewModelType = System.Type;

namespace NapCatScript.Desktop.NetWorkViews;

/// <summary>
/// 对于MinViews 需要去 ViewLocator进行注册 ， 因为其属于根界面
/// </summary>
public class CreateNetWorkViewLocator : IDataTemplate
{
    static CreateNetWorkViewLocator()
    {
        ViewModelMap.Add(WebSocketClientViewModel.Type, typeof(CreateWebSocketClientView));
        ViewModelMap.Add(WebSocketServerViewModel.Type, typeof(CreateWebSocketServerView));
        ViewModelMap.Add(HttpSseServerViewModel.Type, typeof(CreateHttpSseServerView));
        ViewModelMap.Add(HttpClientViewModel.Type, typeof(CreateHttpClientView));
        ViewModelMap.Add(HttpServerViewModel.Type, typeof(CreateHttpServerView));
        ViewModelMap.Add(typeof(ListViewModel), typeof(ListView));
        ViewModelMap.Add(typeof(NetWorkCreateViewModel), typeof(NetWorkCreateView));
    }

    public static Dictionary<ViewModelType, ViewType> ViewModelMap { get;} = new Dictionary<Type, Type>();
    public Control? Build(object? param)
    {
        if (param is null)
            return null;
        ViewType viewType = param.GetType();
        if (ViewModelMap.TryGetValue(viewType, out var view)) {
            return (Control)Activator.CreateInstance(view)!;
        }
        return new TextBlock { Text = "Not Found: " + viewType.FullName };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}
