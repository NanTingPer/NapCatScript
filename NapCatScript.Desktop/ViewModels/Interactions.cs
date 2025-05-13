using System.Reactive;
using ReactiveUI;

namespace NapCatScript.Desktop.ViewModels;

public static class Interactions
{
    /// <summary>
    /// WebUI连接的交互
    /// </summary>
    public static Interaction<(string, string), Unit> WebUIConnectionEvent {get;} = new();
}