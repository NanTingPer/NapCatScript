using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using NapCatScript.Desktop.ViewModels;
using NapCatScript.Desktop.ViewModels.NetWorkModels;
using NapCatScript.Desktop.Views;
using NapCatScript.Desktop.Views.NetWorkViews;
using ViewType = System.Type;
using ViewModelType = System.Type;

namespace NapCatScript.Desktop;

public class ViewLocator : IDataTemplate
{
    static ViewLocator()
    {
        ViewModelMap.Add(typeof(HttpServerViewModel), typeof(HttpServerView));
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
