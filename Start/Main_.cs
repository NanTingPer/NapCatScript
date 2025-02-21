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
            Task.Run(async () =>
            {
                await 建立连接(Socket, useUri ??= "1");
                while(true) {
                    await Task.Delay(1);
                    NoPMesgList.Add(await Socket.ReceiveAsync());
                }
            });

            while (!IsConnection) {
                Thread.Sleep(3);
            }

            Task.Run(() =>
            {

            });

        }

        private static async Task 建立连接(ClientWebSocket socket, string uri)
        {
            try {
                await socket.ConnectAsync(new Uri(uri), CTokrn);
                IsConnection = true;
            } catch {
                throw new Exception("建立连接: 请检查URI是否有效，服务是否正常可访问");
            }
            SetConf(URI, uri);
        }
    }
}
