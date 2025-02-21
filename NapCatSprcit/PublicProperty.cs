using NapCatSprcit.MessagesService;
using NapCatSprcit.WebSocketConnection;

namespace NapCatSprcit
{
    /// <summary>
    /// 公共属性
    /// <para> 包括 wsURI HttpURL </para>
    /// </summary>
    public class PublicProperty
    {
        public static string WebSocketURI = "ws://127.0.0.1:3001";
        public static string HttpURI = "http://127.0.0.1:25511/";
        public static Messages? Messages;
        public static Connection? Connection;
        public static string DatabasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"message.db");
    }

}
