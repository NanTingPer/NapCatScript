using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia.ReactiveUI;
using NapCatScript.Desktop.ViewModels;
using NapCatScript.Desktop.ViewModels.NetWorkModels;
using ReactiveUI;

namespace NapCatScript.Desktop.Views.NetWorkViews.MiniViews;

public abstract class MinView<TViewModel> : ReactiveUserControl<TViewModel>
    where TViewModel : ViewModelBase, ICofnigModel 
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

    /// <summary>
    /// 每当所属<see cref="TViewModel"/>的值属性(含<see cref="string"/>)发生变化时，调用此方法
    /// </summary>
    protected virtual void WhenAnyProperty()
    {
        object obj = ViewModel.GetNetWork();
        NetWorkInteraction.UpdateServerInteraction.Handle((obj, obj.GetType())).Subscribe();
    }

    private void WhenAnyMethod(object? o1)
    {
        if(_isInitialized)
            WhenAnyProperty();
    }
}