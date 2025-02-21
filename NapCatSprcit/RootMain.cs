using NapCatSprcit.MessagesService;
using NapCatSprcit.MessagesService.MessagesProcess;
using NapCatSprcit.WebSocketConnection;

namespace NapCatSprcit
{
    public class RootMain
    {
        public static bool isOpen = true;

        private static List<string> messagesList = new List<string>();
        static void Main(string[] args)
        {
            //if (args.Count() > 0)
            //{
            //    PublicProperty.WebSocketURI = args[0];
            //    PublicProperty.HttpURI = args[1];
            //}
            //else
            //{
            //    do
            //    {
            //        #region 测试放开

            //        Console.WriteLine("请输入WebSocketURI");
            //        string? w = Console.ReadLine();
            //        if (w == null)
            //        {
            //            continue;
            //        }
            //        PublicProperty.WebSocketURI = w;

            //        if (PublicProperty.WebSocketURI == null)
            //            continue;

            //        Console.WriteLine("请输入HttpURI");
            //        string? r = Console.ReadLine();
            //        if (r == null)
            //        {
            //            continue;
            //        }
            //        PublicProperty.HttpURI = r;

            //        #endregion

            //    } while (PublicProperty.HttpURI == "" || PublicProperty.WebSocketURI == "");

            //}


            PublicProperty.Connection = new Connection();
            PublicProperty.Connection.InitionWebSocket().GetAwaiter().GetResult();
            PublicProperty.Messages = new Messages();


            //接收消息
            Task.Run(async () =>
            {
                Console.WriteLine("RootMain : 开始接收消息");
                while (true)
                {
                    IAsyncEnumerable<string> strs = PublicProperty.Messages.ReceiveMes();
                    await foreach (string str in strs)
                    {
                        messagesList.Add(str);
                    }
                }
            });

            //判断结束
            Task.Run(async () =>
            {
                while (true)
                {
                    string? e = Console.ReadLine();
                    if (e != null && (e == "quit" || e == "exit"))
                    {
                        await PublicProperty.Connection.CloseWebSocket();
                        Console.WriteLine("bye -=-");
                        Environment.Exit(0);
                    }
                }
            });

            Task.Run(async () =>
            {
                while(true)
                {
                    if(messagesList.Count > 0)
                    {
                        await ProcessClass.MessagesProcess(messagesList);
                    }
                }
            });

            #region 基础消息发送
            /*
            Task.Run(async () =>
            {
                int isExit = 0;
                while (true)
                {
                    await Task.Delay(1000);
                    string content = await messages.SendPostMessagesAsync("2375948110", "Hello");
                    if(!string.IsNullOrEmpty(content))
                    {
                        Console.WriteLine("发送结果:");
                        Console.WriteLine(content + "\n");
                    }
                    isExit++;
                    if (isExit > 3)
                        return;
                }
            });
            */
            #endregion

            while (true)
            {
                //if (messagesList.Count > 0)
                //{
                //    Console.WriteLine(messagesList[0] + "\n");
                //    messagesList.RemoveAt(0);
                //}
            }
            
        }

    }
}

#region 其实也是正确的
/*

using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json.Nodes;

namespace NapCatSprcit
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
            Conn();
            while (true)
            { }
        }

        public static async void Conn()
        {
            using (ClientWebSocket webSocket = new ClientWebSocket())
            {
                //accessToken
                //webSocket.Options.SetRequestHeader("Authorization", "Bearer nantingttt");

                Uri uri = new Uri("ws://122.11.29.203:3001");
                CancellationToken cts = new CancellationToken();
                //链接服务
                await webSocket.ConnectAsync(uri, cts);
                Console.WriteLine("等待");
                Console.WriteLine("开启");

                byte[] str = new byte[byte.Parse("2")];

                var receiveData = new ArraySegment<byte>(str);
                var stb = new StringBuilder();
                while (true)
                {
                    webSocket.ReceiveAsync(receiveData, cts);
                    await Task.Delay(22);
                    stb.Append(Encoding.ASCII.GetString(receiveData.ToArray(), receiveData.Offset, receiveData.Count));
                    if(stb.ToString() is not "") Console.WriteLine(stb.ToString() + "\n\n");
                        stb.Clear();
                }
                await webSocket.CloseAsync(webSocket.CloseStatus.Value, "wu", cts);
            }

        }
    }
}
 


 */
#endregion

#region 正确的
//Task.Run(async () =>
//{
//    using (var clink = new ClientWebSocket())
//    {

//        var ct = new CancellationToken();

//        do
//        {
//            await clink.ConnectAsync(new Uri("ws://122.11.29.203:3001"), ct);
//            Console.WriteLine("握手");
//        } while (clink.State != WebSocketState.Open);

//        Console.WriteLine("握手完成");
//        Console.WriteLine(clink.State);

//        byte[] bytes = new byte[1024 * 10];
//        var ars = new ArraySegment<byte>(bytes);
//        while (true)
//        {
//            if(isOpen == false)
//            {
//                await clink.CloseAsync(clink.CloseStatus.Value,"hand",ct);
//                Console.WriteLine("已经关闭");
//            }

//            WebSocketReceiveResult result = await clink.ReceiveAsync(ars, ct);

//            if (result.EndOfMessage)
//            {
//                Console.WriteLine(result.MessageType.ToString());
//                string jsonStr = Encoding.UTF8.GetString(bytes, ars.Offset, ars.Count);
//                //JsonNode jsonObject = JsonObject.Parse(jsonStr);
//                Console.WriteLine(jsonStr);
//        }
//    }
//    }
//});

//while (true)
//{
//    string? r = Console.ReadLine();
//    if (r != null && r == "quit")
//    {
//        isOpen = false;
//    }
//}
#endregion