#pragma warning disable CA2255
using NapCatScript.Core.MsgHandle;
using NapCatScript.Core.Services;
using NapCatScript.Start;
using System.Runtime.CompilerServices;
using static NapCatScript.Core.Services.Config;
namespace NapCatScript.Core;
public static class CoreConfigValueAndObject
{
#pragma warning disable CS8618 // 请考虑添加 "required" 修饰符或声明为可为 null。
    /// <summary>
    /// 用户配置的Uri，这个决定WebSocket链接
    /// </summary>
    public static string SocketUri { get; private set; }
    /// <summary>
    /// 基础请求uri http://127.0.0.1:6666
    /// </summary>
    public static string HttpUri { get; set; }
    public static string RootId { get; set; }
    public static string BotId { get; set; }
    public static ClientWebSocket Socket { get; private set; } = new ClientWebSocket();
    public static CancellationToken CTokrn { get; } = new CancellationToken();

    public static Send SendObject { get; private set; }
#pragma warning restore CS8618 // 请考虑添加 "required" 修饰符或声明为可为 null。
    public static List<PluginType> Plugins { get; } = [];

    [ModuleInitializer]
    internal static void CoreConfigLoad()
    {
        InstanceLog.Info("加载核心配置!");
        string? useUri = GetConf(URI);
        string? httpUri = GetConf(HttpURI);
        BotId = GetConf(BootId) ?? "";
        if (string.IsNullOrEmpty(useUri) || string.IsNullOrEmpty(httpUri)) {
            InstanceLog.Waring("配置文件已生成，请检查Uri配置");
            Console.ReadLine();
            Environment.Exit(0);
        }
        SocketUri = useUri;
        SocketUri ??= "1";
        HttpUri = httpUri;
        RootId = GetConf(Config.RootId) ?? "";
        SendObject = new Send(HttpUri);

        PluginLoad.LoadPlugin(Plugins);
    }
}
