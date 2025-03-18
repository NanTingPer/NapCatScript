using NapCatScript.JsonFromat;
using NapCatScript.JsonFromat.Mesgs;
using NapCatScript.MesgHandle;
using NapCatScript.MesgHandle.Parses;
using System.Net.WebSockets;
using System.Text;
using static NapCatScript.MesgHandle.Parses.ReceiveMesg;
using static NapCatScript.Tool.Config;
using HUtils = NapCatScript.MesgHandle.Utils;

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
    public static string HttpUri { get; set; } = "";
    public static ClientWebSocket Socket { get; } = new ClientWebSocket();
    public static CancellationToken CTokrn { get; } = new CancellationToken();
    public static List<MesgInfo> NoPMesgList { get; } = [];
    public static bool IsConnection = false;
    public static Random rand = new Random();
    static void Main(string[] args)
    {
        string? useUri = GetConf(URI);
        string? httpUri = GetConf(HttpURI);
        if (string.IsNullOrEmpty(useUri) || string.IsNullOrEmpty(httpUri)) {
            Console.WriteLine("请检查Uri配置");
            return;
        }
        SocketUri = useUri;
        HttpUri = httpUri;
        //接收消息 并将有效消息存放到NoPMesgList
        Task.Run(async () =>
        {
            await 建立连接(Socket, useUri ??= "1");
            while(true) {
                await Task.Delay(1);
                MesgInfo? mesg = await Socket.ReceiveAsync(CTokrn); //收到的消息
                if(mesg is not null) {
                    NoPMesgList.Add(mesg);
                    Console.WriteLine(mesg);
                }
            }
        });

        //while (!IsConnection) {
        //    Thread.Sleep(3);
        //}

        //发送消息
        Task.Run(async () =>
        {
            while (true) {
                await Task.Delay(1);
                if (NoPMesgList.Count <= 0)
                    continue;
                MesgInfo mesg = NoPMesgList.First();
                NoPMesgList.RemoveAt(0);
                string mesgContent = mesg.MessageContent;
                if (mesgContent.Trim().StartsWith('.')) {
                    string[] mesgs = mesgContent.Split(".");
                    string txtContent = mesgs[1];//消息内容
                    if (txtContent.StartsWith("映射#")) {
                        await CalMapping.Add(mesg, httpUri, txtContent, CTokrn);
                        continue;
                    }

                    txtContent = await CalMapping.GetMap(txtContent);
                    string filePath = Path.Combine(Environment.CurrentDirectory, "Cal", txtContent + ".png");
                    string sendUrl = HUtils.GetMsgURL(HttpUri, mesg, out MesgTo MESGTO);
                    await Send.SendCalImage(mesg, txtContent, filePath, sendUrl, MESGTO);
                }
            }
        });

        while(true) {
            Thread.Sleep(1000);
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
            Environment.Exit(0);
        }
        SetConf(URI, uri);
    }
}
