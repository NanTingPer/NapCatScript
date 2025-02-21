using NapCatSprcit.WebSocketConnection;
using System.Net.WebSockets;

namespace NapCatSprcit.MessagesService
{
    public partial class Messages
    {
        /// <summary>
        /// 连接
        /// </summary>
        private Connection _connection;

        /// <summary>
        /// WebSocket连接
        /// </summary>
        private ClientWebSocket _clientWebSocket;
    }
}
