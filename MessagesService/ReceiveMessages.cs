using NapCatSprcit.WebSocketConnection;
using System.Net.WebSockets;
using System.Text;

namespace NapCatSprcit.MessagesService
{
    public partial class Messages
    {
        public Messages() 
        {
            _connection = PublicProperty.Connection;
            _clientWebSocket = _connection.PublicConnection;
            Console.WriteLine("NapCatSprcit.MessagesService.Messages : 构建Messages");
        }

        /// <summary>
        /// 接收并返回数据
        /// </summary>
        /// <returns></returns>
        public async IAsyncEnumerable<string> ReceiveMes()
        {
            Console.WriteLine("NapCatSprcit.MessagesService.Messages : ReceiveMes 开始接收消息");
            Console.WriteLine(PublicProperty.Connection.PublicConnection.State);
            byte[] dataByte = new byte[1024 * 10];
            ArraySegment<byte> data = new ArraySegment<byte>(dataByte);
            while (true)
            {
                if(_connection.State == WebSocketState.Closed)
                {
                    Console.WriteLine("NapCatSprcit.MessagesService.Messages : ReceiveMes 连接已关闭");
                    yield break;
                }

                //_clientWebSocket
                WebSocketReceiveResult result = await PublicProperty.Connection.PublicConnection.ReceiveAsync(data, Connection.WebSocketCT);
                if (result.EndOfMessage)
                {
                    yield return Encoding.UTF8.GetString(dataByte, data.Offset, result.Count);
                }
            }
        }

        //public async IEnumerable<string> GetMessagesString()
        //{
        //    while (true)
        //    {
        //        IAsyncEnumerable<string> iae = ReceiveMes();
        //        await foreach(var str in iae)
        //        {
        //            yield return str;
        //        }
        //    }
    }
}
