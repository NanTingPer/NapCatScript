namespace MesgHandle
{
    /// <summary>
    /// 数据信息 仅适用于接收消息
    /// </summary>
    public class MesgInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public ulong UserId { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string MessageContent { get; set; } = string.Empty;

        /// <summary>
        /// 消息类型
        /// </summary>
        public string MessageType { get; set; } = string.Empty;

        public override string ToString()
        {

            return 
                "------------------------------------------------\r\n" +
                UserId + "\r\n" + 
                MessageContent + "\r\n" + 
                MessageType + "\r\n" +
                "------------------------------------------------\r\n";
        }
    }
}