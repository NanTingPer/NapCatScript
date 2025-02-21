namespace MesgHandle
{
    /// <summary>
    /// 数据信息
    /// </summary>
    public class MessagesInfo
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

            return UserId + " " + MessageContent + " " + MessageType;
        }
    }
}