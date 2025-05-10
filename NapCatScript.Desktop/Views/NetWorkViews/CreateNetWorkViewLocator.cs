using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using NapCatScript.Desktop.ViewModels;
using NapCatScript.Desktop.ViewModels.NetWorkModels;
using NapCatScript.Desktop.Views;
using NapCatScript.Desktop.Views.NetWorkViews;
using NapCatScript.Desktop.Views.NetWorkViews.CreateViews;
using ViewType = System.Type;
using ViewModelType = System.Type;

namespace NapCatScript.Desktop.Views.NetWorkViews;

public class CreateNetWorkViewLocator : IDataTemplate
{
    static CreateNetWorkViewLocator()
    {
        ViewModelMap.Add(typeof(HttpServerViewModel), typeof(CreateHttpServerView));
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
