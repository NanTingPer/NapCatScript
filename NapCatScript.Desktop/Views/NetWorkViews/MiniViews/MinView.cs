using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using NapCatScript.Desktop.ViewModels;
using ReactiveUI;

namespace NapCatScript.Desktop.Views.NetWorkViews.MiniViews;

public abstract class MinView<TViewModel> : ReactiveUserControl<TViewModel>
    where TViewModel : ViewModelBase
{
    private bool _isInitialized = false;
    protected MinView()
    {
        this.WhenActivated(WhenAllProperty);
    }
    
    private async void WhenAllProperty(CompositeDisposable action)
    {
        Type viewModelType = typeof(TViewModel);
        
        var BIP = BindingFlags.Instance | BindingFlags.Public;
        var props = typeof(TViewModel).GetProperties(BIP).Where(f => f.PropertyType.IsValueType || f.PropertyType == typeof(string)); //ViewModel全部属性
        foreach (var info in props) {
            //f
            ParameterExpression 参数 = Expression.Parameter(viewModelType, "f");
            
            //f.Name
            var 成员树 = Expression.Property(参数, info); //访问typof(TThis)的 info属性
            
            //转换树 (object)f.info
            var ConvertExpr = Expression.Convert(成员树, typeof(object));
            
            //f => (object)f.name
            var exp = Expression.Lambda<Func<TViewModel, object>>(ConvertExpr, [参数]);

            List<Expression> list = [];
            
            //this.WhenAnyDynamic(成员树, f => (object?)f.Value).Subscribe(WhenAnyMethod);
            this.ViewModel
                .WhenAnyValue(exp)
                .WhereNotNull()
                .Throttle(TimeSpan.FromSeconds(0.5))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe(WhenAnyMethod)
                .DisposeWith(action);
        }

        await Task.Delay(1000);
        _isInitialized = true;
    }

    protected void WhenAnyMethod(object? o1)
    {
        if(_isInitialized)
            Debug.WriteLine("UpDate: " + o1.ToString());
    }
}