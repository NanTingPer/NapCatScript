using System;
using System.Collections.Generic;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using NapCatScript.Desktop.NetWorkViews;
using NapCatScript.Desktop.NetWorkViews.MiniViews;
using NapCatScript.Desktop.ViewModels;
using NapCatScript.Desktop.ViewModels.NetWorkModels;
using NapCatScript.Desktop.Views;
using NapCatScript.Desktop.Views.LoginView;
using NapCatScript.Desktop.Views.NetWorkViews;
using NapCatScript.Desktop.Views.NetWorkViews.MiniViews;
using BindingFlags = System.Reflection.BindingFlags;
using ViewType = System.Type;
using ViewModelType = System.Type;

namespace NapCatScript.Desktop;

public class ViewLocator : IDataTemplate
{
    static ViewLocator()
    {
        ViewModelMap.Add(typeof(NetWorkViewModel), typeof(NetWorkView));
        ViewModelMap.Add(typeof(WebUiLoginInfo), typeof(WebUiLoginView));
        ViewModelMap.Add(HttpSseServerViewModel.Type, typeof(HttpSseServerView));
        ViewModelMap.Add(HttpServerViewModel.Type, typeof(HttpServerView));
        ViewModelMap.Add(HttpClientViewModel.Type, typeof(HttpClientView));
        ViewModelMap.Add(WebSocketServerViewModel.Type, typeof(WebSocketServerView));
        ViewModelMap.Add(WebSocketClientViewModel.Type, typeof(WebSocketClientView));
    }

    public static Dictionary<ViewModelType, ViewType> ViewModelMap { get;} = new Dictionary<Type, Type>();
    public Control? Build(object? param)
    {
        if (param is null)
            return null;
        ViewType viewType = param.GetType();
        if (ViewModelMap.TryGetValue(viewType, out var view)) {
            //ConstructorInfo ctor = view.GetConstructor(BindingFlags.Instance | BindingFlags.Public, []);
            //var obje = ctor.Invoke([]);
            
            var obj = Activator.CreateInstance(view)!;
            //var obj = (object)new HttpClientView();
            
            return (Control)obj;
        }
        
        var name = viewType.FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        var type = Type.GetType(name);

        if (type != null) {
            return (Control)Activator.CreateInstance(type)!;
        }
        
        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}
