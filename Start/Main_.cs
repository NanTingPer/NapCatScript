using System.Net.WebSockets;
using static Start.Config;

namespace Start
{
    public class Main_
    {
        public static ClientWebSocket Socket { get; } = new ClientWebSocket();
        public static CancellationToken CTokrn { get; } = new CancellationToken();
        public static List<string> NoPMesgList { get; } = [];
        public static bool IsConnection = false;
        static void Main(string[] args)
        {
            string? useUri;
            useUri = GetConf(URI);
            if (string.IsNullOrEmpty(useUri)) {
                Console.WriteLine("请检查Uri配置");
                return;
            }
            Task.Run(async () =>
            {
                await 建立连接(Socket, useUri ??= "1");
                while(true) {
                    await Task.Delay(1);
                    string? r = await Socket.ReceiveAsync();
                    if(r is not null) {
                        NoPMesgList.Add(r);
                        Console.WriteLine(r);
                    }
                }
            });

            while (!IsConnection) {
                Thread.Sleep(3);
            }

            Task.Run(() =>
            {

            });

            while(true) {
                Thread.Sleep(1000);
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
}
