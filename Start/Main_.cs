using NapCatScript.JsonFromat;
using NapCatScript.MesgHandle.Parses;
using System.Net.WebSockets;
using static NapCatScript.MesgHandle.Parses.ReceiveMesg;
using static NapCatScript.MesgHandle.Utils;
using static NapCatScript.Start.FAQ;
using HUtils = NapCatScript.MesgHandle.Utils;
using Config = NapCatScript.Services.Config;
using NapCatScript.Start.Handles;

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
    static void Main(string[] args)
    {
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
            Task.Run(async () => {
                await 建立连接(Socket, useUri ??= "1");
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
            });

            //while (!IsConnection) {
            //    Thread.Sleep(3);
            //}

            //发送消息
            Task.Run(async () => {
                while (true) {
                    await Task.Delay(1);
                    if (NoPMesgList.Count <= 0)
                        continue;

                    MesgInfo mesg = NoPMesgList.First();
                    NoPMesgList.RemoveAt(0);
                    string mesgContent = mesg.MessageContent;
                    Log.Info(mesg);
                    MService.SetAsync(mesg);
                    mesgContent = Regex.Replace(mesgContent, @"\s", "");
                    if (!mesgContent.StartsWith("亭亭$亭"))
                        DeepSeekAPI.AddGroupMesg(mesg); //加入组
                    if (mesgContent.StartsWith(StartString) || mesgContent.StartsWith("亭亭")) {
                        try {
                            DeepSeekAPI.SendAsync(mesg, httpUri, mesgContent, CTokrn);
                        } catch (Exception E) {
                            Console.WriteLine($"DeepSeek错误: {E.Message} \r\n {E.StackTrace}");
                            Log.Erro(E.Message, E.StackTrace);
                        }
                        continue;
                    }

                    var co = await FAQI.Get(mesgContent);
                    if (co is not null) {
                        SendTextAsync(mesg, HttpUri, $"{co.Value}\r\n----来自:{co.UserName}", CTokrn);
                        continue;
                    }

                    if (mesgContent.Trim().StartsWith('.')) {
                        //string[] mesgs = mesgContent.Split(".");
                        string txtContent/* = mesgs[1]*/;//消息内容
                        txtContent = mesgContent.Trim().Substring(1);
                        if (txtContent.StartsWith("映射#")) {
                            CalMapping.AddAsync(mesg, HttpUri, txtContent, CTokrn);
                            continue;
                        } else if (txtContent.StartsWith("删除映射#")) {
                            CalMapping.DeleteAsync(mesg, HttpUri, txtContent, CTokrn);
                            continue;
                        } else if (txtContent.StartsWith("FAQ#")) {
                            FAQI.AddAsync(mesg, HttpUri, txtContent, CTokrn);
                            continue;
                        } else if (txtContent.StartsWith("删除FAQ#")) {
                            FAQI.DeleteAsync(txtContent);
                            SendTextAsync(mesg, HttpUri, "好啦好啦，删掉啦", CTokrn);
                            continue;
                        } else if (txtContent.StartsWith("help#")) {
                            SendTextAsync(mesg, HttpUri,
                                """
                            对于灾厄Wiki: 
                                1. 使用"." + 物品名称 可以获得对应物品的wiki页, 例 .震波炸弹
                                2. 使用".映射#" 可以设置对应物品映射, 例   .映射#神明吞噬者=>神吞
                                3. 使用".删除映射#" 可以删除对应映射, 例   .删除映射#神吞
                            对于FAQ:
                                1. 使用".FAQ#" 可以创建FAQ     例      .FAQ#灾厄是什么###灾厄是一个模组
                                2. 使用".删除FAQ#" 可以删除FAQ 例      .删除FAQ#灾厄是什么
                            
                            """, CTokrn);
                            continue;
                        }

                        txtContent = await CalMapping.GetMap(txtContent);
                        string filePath = Path.Combine(Environment.CurrentDirectory, "Cal", txtContent + ".png");
                        string sendUrl = HUtils.GetMsgURL(HttpUri, mesg, out MesgTo MESGTO);
                        CalImage.SendAsync(mesg, txtContent, filePath, sendUrl, MESGTO);
                    }
                }
            });

            while (true) {
                _ = Console.ReadLine();
            }
        } catch (Exception e) {
            Log.Erro(e.Message + "\r\n" + e.StackTrace);
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
