using NapCatScript.JsonFromat;
using NapCatScript.MesgHandle.Parses;
using System.Net.WebSockets;
using static NapCatScript.MesgHandle.Parses.ReceiveMesg;
using static NapCatScript.MesgHandle.Utils;
using static NapCatScript.Start.FAQ;
using HUtils = NapCatScript.MesgHandle.Utils;
using Config = NapCatScript.Services.Config;
using NapCatScript.Start.Handles;
using System.Runtime.Loader;
using System.Reflection;
using NapCatScript.Services;

namespace NapCatScript.Start;

/// <summary>
/// 
/// </summary>
public class Main_
{
    /// <summary>
    /// 用户配置的Uri，这个决定WebSocket链接
    /// </summary>
    public static string SocketUri { get; private set; } = "";
    /// <summary>
    /// 基础请求uri http://127.0.0.1:6666
    /// </summary>
    public static string HttpUri { get; set; } = "";
    public static string DeepSeekKey { get; set; } = "";
    public static string RootId { get; set; } = "";
    public static string BotId { get; set; } = "";
    public static string StartString { get; set; } = "";
    public static ClientWebSocket Socket { get; } = new ClientWebSocket();
    public static CancellationToken CTokrn { get; } = new CancellationToken();
    public static List<MesgInfo> NoPMesgList { get; } = [];
    public static bool IsConnection = false;
    public static Random rand = new Random();
    public static List<PluginType> Plugins = [];
    static void Main(string[] args)
    {
        LoadPlugin();
        #region Main
        try {
            string? useUri = GetConf(URI);
            string? httpUri = GetConf(HttpURI);
            BotId = GetConf(BootId) ?? "";
            StartString = $"[CQ:at,qq={BotId}]";
            StartString = Regex.Replace(StartString, @"\s", "");
            if (string.IsNullOrEmpty(useUri) || string.IsNullOrEmpty(httpUri)) {
                Console.WriteLine("请检查Uri配置");
                return;
            }
            SocketUri = useUri;
            HttpUri = httpUri;
            DeepSeekKey = GetConf(Config.DeepSeekKey) ?? "";
            RootId = GetConf(Config.RootId) ?? "";
            //接收消息 并将有效消息存放到NoPMesgList
            Task.Run(Receive);

            //发送消息
            Task.Run(Send);

            while (true) {
                _ = Console.ReadLine();
            }

        } catch (Exception e) {
            Log.Erro(e.Message + "\r\n" + e.StackTrace);
        }


    }
    #endregion Main

    private static async void Receive()
    {
        await 建立连接(Socket, SocketUri ??= "1");
        while (true) {
            await Task.Delay(1);
            try {
                MesgInfo? mesg = await Socket.Receive(CTokrn); //收到的消息
                if (mesg is not null) {
                    NoPMesgList.Add(mesg);
                    Console.WriteLine(mesg);
                }
            } catch (Exception e) {
                Console.WriteLine($"消息接收发生错误: {e.Message}\r\n {e.StackTrace}");
                Log.Erro("消息接收发生错误: ", e.Message, e.StackTrace);
            }

        }
    }

    private static async void Send()
    {
        while (true) {
            await Task.Delay(1);
            if (NoPMesgList.Count <= 0)
                continue;

            MesgInfo mesg = NoPMesgList.First();
            NoPMesgList.RemoveAt(0);
            Log.Info(mesg);
            MService.SetAsync(mesg);
            Plugins.ForEach(f => f.Run(mesg, HttpUri));
        }
    }

    private static void LoadPlugin()
    {
        string pluginDirectory = Path.Combine(Environment.CurrentDirectory, "Plugin");
        if (!Directory.Exists(pluginDirectory))
            Directory.CreateDirectory(pluginDirectory);
        string[] plugins = Directory.GetDirectories(pluginDirectory); //给的是绝对路径
        foreach (var pluginPath in plugins) {
            string pluginName = new DirectoryInfo(pluginPath).Name;
            string pluginDllPath = Path.Combine(pluginPath, pluginName + ".dll");
            PluginLoad plugin = new PluginLoad(pluginDllPath);
            Assembly ass = plugin.LoadFromAssemblyPath(pluginDllPath);
            Type? type = ass.GetTypes().Where(f => f.Name == "TestClass").FirstOrDefault();
            if (type is null) return;
            ConstructorInfo? pluginConstructor = type.GetConstructors().FirstOrDefault(f => f.GetParameters().Length == 0);
            if (pluginConstructor is null) return;
            PluginType obj = (PluginType)pluginConstructor.Invoke(null);
            obj.Init();
            Plugins.Add(obj);
        }
    }

    /// <summary>
    /// 往sbuilder添加字符串
    /// </summary>
    public static void AddString(StringBuilder sbuilder, params IEnumerable<string>[] ies)
    {
        foreach (var ie in ies) {
            foreach (var str in ie) {
                sbuilder.Append(str + ", ");
            }
        }
    }

    private static async Task 建立连接(ClientWebSocket socket, string uri)
    {
        try {
            await socket.ConnectAsync(new Uri(uri), CTokrn);
            Console.WriteLine("连接成功");
            IsConnection = true;
        } catch (Exception e){
            Console.WriteLine(e.Message);
            Console.WriteLine("建立连接: 请检查URI是否有效，服务是否正常可访问");
            Log.Erro(e.Message, e.StackTrace, "建立连接: 请检查URI是否有效，服务是否正常可访问");
            Environment.Exit(0);
        }
        SetConf(URI, uri);
    }
}
