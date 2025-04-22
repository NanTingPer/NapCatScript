using System.Net.WebSockets;
using static NapCatScript.MesgHandle.ReceiveMesg;
using Config = NapCatScript.Services.Config;
using System.Reflection;
using NapCatScript.Services;
using NapCatScript.MesgHandle;
using NapCatScript.JsonFromat.Msgs;
using NapCatScript.JsonFromat;
using System.Net.Sockets;
using System;
using System.Threading.Tasks;
using System.Data;

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
    public static string RootId { get; set; } = "";
    public static string BotId { get; set; } = "";
    public static ClientWebSocket Socket { get; private set; } = new ClientWebSocket();
    public static CancellationToken CTokrn { get; } = new CancellationToken();
    public static List<MesgInfo> NoPMesgList { get; } = [];
    public static bool IsConnection = false;
    public static Random rand = new Random();
    public static List<PluginType> Plugins = [];
    public static long lifeTime = 0;
    public static long oldLifeTime = 0;
    public static long seconds = 0;

    /// <summary>
    /// ws状态
    /// </summary>
    public static ConnectionState state = ConnectionState.Open;
    public static Send SendObject { get; private set; }
    static Main_()
    {
        string? useUri = GetConf(URI);
        string? httpUri = GetConf(HttpURI);
        BotId = GetConf(BootId) ?? "";
        if (string.IsNullOrEmpty(useUri) || string.IsNullOrEmpty(httpUri)) {
            Console.WriteLine("配置文件已生成，请检查Uri配置");
            Console.ReadLine();
            Environment.Exit(0);
        }
        SocketUri = useUri;
        HttpUri = httpUri;
        RootId = GetConf(Config.RootId) ?? "";
        SendObject = new Send(HttpUri);
    }
    static void Main(string[] args)
    {
        LoadPlugin();
        try {
            //接收消息 并将有效消息存放到NoPMesgList
            Task.Run(Receive);
#if DEBUG
            //发送消息
            Task.Run(Send);
#endif

#if RELEASE
            //发送消息
            Task.Run(Send);
#endif
            //心跳
            Task.Run(LifeCycle);

            while (true) {
                _ = Console.ReadLine();
            }
        } catch (Exception e) {
            Log.Erro(e.Message + "\r\n" + e.StackTrace);
        }
    }

    /// <summary>
    /// 建立链接并接受消息
    /// </summary>
    private static async void Receive()
    {
        await 建立连接(Socket, SocketUri ??= "1");
        while (true) {
            await Task.Delay(1);
            try {
                MesgInfo? mesg = await Socket.Receive(CTokrn); //收到的消息
                if (mesg is not null) {
                    if (mesg.lifeTime != 0)
                        SetLifeTime(mesg.lifeTime);
                    NoPMesgList.Add(mesg);
                    //Console.WriteLine(mesg);
                }
            } catch (Exception e) {
                Log.Erro("消息接收发生错误: ", e.Message, e.StackTrace);
            }

        }
    }

    /// <summary>
    /// 每收到消息 就发送
    /// </summary>
    private static async void Send()
    {
        Send sned = new Send(HttpUri);
        while (true) {
            await Task.Delay(1);
            if (NoPMesgList.Count <= 0)
                continue;
            MesgInfo mesg = NoPMesgList.First();
            //interfaceTest(sned); // Test
            NoPMesgList.RemoveAt(0);
            Log.Info(mesg);
            MService.SetAsync(mesg);
            foreach (var pType in Plugins) {
                try {
                    pType.Run(mesg, HttpUri);
                } catch (Exception e) {
                    Log.Erro($"插件:{pType.GetType().FullName}", e.Message, e.StackTrace);
                }
            }
        }
    }

    private static void interfaceTest(Send send)
    {
        var contents = new List<MsgJson>()
        {
            new AtJson("qqid"),
            new TextJson("sendMsgText"),
        };
        send.SendMsg("qqid", MsgTo.user, contents);
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
            Assembly ass = plugin.LoadFromAssemblyPath(pluginDllPath); //加载插件程序集
            #region 初始化插件的全部PluginType
            IEnumerable<Type> pluginStartTypes = ass.GetTypes().Where(f => typeof(PluginType) == f.BaseType);
            foreach (var pluginStartType in pluginStartTypes) {
                ConstructorInfo? pluginConstructor = pluginStartType.GetConstructors().FirstOrDefault(f => f.GetParameters().Length == 0);
                if (pluginConstructor is null) return;
                PluginType obj = (PluginType)pluginConstructor.Invoke(null);
                obj.Init();
                Plugins.Add(obj);
            }
            #endregion
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
            Log.Erro("建立连接: 请检查URI是否有效，服务是否正常可访问", e.Message, e.StackTrace);
            Environment.Exit(0);
        }
        SetConf(URI, uri);
    }

    /// <summary>
    /// 重新链接
    /// </summary>
    /// <returns>成功返回true</returns>
    private static async Task<bool> ReConnect(string uri)
    {
        try {
            Socket.Abort();
            //await Socket.CloseAsync(WebSocketCloseStatus.Empty, "", CTokrn);
            Socket = new ClientWebSocket();
            await Socket.ConnectAsync(new Uri(uri), CTokrn);
            return true;
        } catch (Exception e) {
            return false;
        }
    }


    /// <summary>
    /// 设置心跳时间
    /// </summary>
    private static void SetLifeTime(long time)
    {
        oldLifeTime = lifeTime;
        lifeTime = time;
    }

    /// <summary>
    /// 心跳
    /// </summary>
    private static async Task LifeCycle()
    {
        while (true) {
            //1000是一秒
            await Task.Delay(1000); //500秒
            seconds++;
            if (!IsConnection)
                continue;
            //DateTime.Now.Ticks
            if(seconds % 9 == 0 && (Socket.State == WebSocketState.Closed || Socket.State == WebSocketState.CloseSent || Socket.State == WebSocketState.CloseReceived || Socket.State == WebSocketState.Aborted)) {
                var temp = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("重新连接");
                Console.ForegroundColor = temp;
                if (await ReConnect(SocketUri))
                    state = ConnectionState.Open;
                else
                    state = ConnectionState.Closed;
            }
        }
    }
}
