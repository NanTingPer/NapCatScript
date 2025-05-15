using System.Reactive;
using ReactiveUI;

namespace NapCatScript.Desktop.ViewModels;

public static class Interactions
{
    /// <summary>
    /// WebUI连接的交互
    /// </summary>
    public static Interaction<(string, string), bool> WebUIConnectionInteraction { get; } = new();
    
    /// <summary>
    /// 删除LoginInfo
    /// </summary>
    public static Interaction<WebUiLoginInfo, Unit> DeleteLoginInfoInteraction { get; } = new();
    
    /// <summary>
    /// 登录WebUi
    /// <para> <see cref="WebUIConnectionViewModel"/> 订阅 </para>
    /// <para> <see cref="WebUiLoginInfo"/> 提供处理程序 </para>
    /// </summary>
    public static Interaction<WebUiLoginInfo, Unit> LoginWebUiInteraction { get; } = new();
}