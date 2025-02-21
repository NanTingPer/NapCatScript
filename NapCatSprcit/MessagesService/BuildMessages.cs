using NapCatSprcit.MessagesService.MessagesProcess;
using NapCatSprcit.MessagesService.SendMessagesClass.AllMsgJsonString;
using System.Text.Json;

namespace NapCatSprcit.MessagesService
{
    public partial class Messages
    {
        /// <summary>
        /// 构建要发送的私聊纯文本 群聊也可以用
        /// </summary>
        /// <param name="user_id"> 目标 </param>
        /// <param name="text"> 文本 </param>
        /// <returns></returns>
        public string buildMessagesText(string user_id, string text)
        {
            var msg = new PriviteAndGroupMsg(user_id, text);
            return JsonSerializer.Serialize(msg.MessagesJsonObject);
        }

        /// <summary>
        /// 构建要发送的私聊纯文本 群聊也可以用
        /// </summary>
        /// <returns></returns>
        public string buildMessagesText(MessagesInfo msinfo, string text)
        {
            var msg = new PriviteAndGroupMsg(msinfo.UserId.ToString(), text);
            return JsonSerializer.Serialize(msg.MessagesJsonObject);
        }


    }
}
