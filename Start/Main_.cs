using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Start
{
    public class Main_
    {
        public static ClientWebSocket Socket { get; } = new ClientWebSocket();
        public static CancellationToken CTokrn { get; } = new CancellationToken();
        public static string Uri { get; set; } = string.Empty;
        static void Main(string[] args)
        {
            string? useUri;
            do
            {
                Console.WriteLine("输入Uri: \t");
                useUri = Console.ReadLine();

                try {
                    new Uri(useUri);
                } catch {
                    useUri = null;
                }

            } while (useUri == null);
            
            建立连接(Socket, new Uri(useUri)).Wait();

            Config.SetConf("SocketUri", useUri);
            Console.WriteLine("Hello, World!");
        }

        private static async Task 建立连接(ClientWebSocket socket, Uri uri)
        {
            try {
                await socket.ConnectAsync(uri, CTokrn);
            } catch {
                throw new Exception("建立连接: 请检查URI是否有效，服务器是否正常可访问");
            }
        }
    }
}
