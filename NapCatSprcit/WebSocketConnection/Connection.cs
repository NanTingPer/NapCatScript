using System.Net.WebSockets;

namespace NapCatSprcit.WebSocketConnection
{
    /// <summary>
    /// 建立连接
    /// </summary>
    public class Connection
    {
        /// <summary>
        /// ws的 状态标识符
        /// </summary>
        public static CancellationToken WebSocketCT { get; } = new CancellationToken();
        
        private ClientWebSocket _clientWebSocket = new ClientWebSocket();

        public Connection()
        {
        }


        public ClientWebSocket PublicConnection 
        {
            get
            {
                //if(_clientWebSocket.State != WebSocketState.None)
                //{
                //    return _clientWebSocket;
                //}
                //else if(_clientWebSocket.State == WebSocketState.None)
                //{
                //    InitionWebSocket();
                //}
                return _clientWebSocket;
            } 
            private set => _clientWebSocket = value;
        }
        public WebSocketState State
        {
            get => getState();
        }

        /// <summary>
        /// 初始化连接
        /// </summary>
        public async Task InitionWebSocket()
        {
            var uri = PublicProperty.WebSocketURI;

            //Uri不正确
            if (string.IsNullOrEmpty(uri))
            {
                Console.WriteLine("URL不正确");
                return;
            }
            await _clientWebSocket.ConnectAsync(new Uri(uri), WebSocketCT);
            Console.WriteLine("已建立链接");
        }

        /// <summary>
        /// 关闭WebSocket连接
        /// </summary>
        public async Task CloseWebSocket()
        {
            await PublicConnection.CloseAsync(WebSocketCloseStatus.EndpointUnavailable, "hand" ,WebSocketCT);
        }

        /// <summary>
        /// 返回连接状态
        /// </summary>
        private WebSocketState getState()
        {
            return PublicConnection.State;
        }
    }
}
