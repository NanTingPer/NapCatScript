using System.Text.Json.Serialization;

namespace NapCatSprcit.MessagesService.SendMessagesClass.AllMsgJsonString
{
    /// <summary>
    /// 图片消息 Json构建
    /// <para> 使用MassageJsonObject字段取出string值 </para>
    /// </summary>
    public class ImageMessage
    {
        public Root MassageJsonObject;

        /// <summary>
        /// 构建
        /// </summary>
        /// <param name="user_id"> 目标ID </param>
        /// <param name="type"> 类型 默认来就行了 </param>
        /// <param name="fileUrl"> 文件链接 / base64 </param>
        public ImageMessage(string user_id,MsgTypeEnum type = MsgTypeEnum.image ,params string[] fileUrl) 
        { 
            List<Message> mesList = new List<Message>();

            foreach (var url in fileUrl)
            {
                if(File.Exists(url))
                {
                    mesList.Add(new Message(type, new Data(ImageToBase64.Method(url))));
                }
            }

            MassageJsonObject = new Root(user_id, mesList);
        }

        /// <summary>
        /// 目标ID
        /// </summary>
        public class Root
        {
            public Root(string user_id,List<Message> messages)
            {
                User_id = user_id;
                Messages = messages;
            }
            [JsonPropertyName("user_id")]
            public string User_id { get; set;} = string.Empty;

            [JsonPropertyName("message")]
            public List<Message> Messages { get; set;} = new List<Message>();
        }

        /// <summary>
        /// 数据类型和内容
        /// </summary>
        public class Message
        {
            public Message(MsgTypeEnum type, Data data) 
            { 
                Data = data;
                Type = type.ToString();
            }

            [JsonPropertyName("type")]
            public string Type { get; set; } = MsgTypeEnum.image.ToString();

            [JsonPropertyName("data")]
            public Data Data { get; set; }

        }

        /// <summary>
        /// 内容
        /// </summary>
        public class Data
        {
            public Data(string file)
            {
                File = file;
            }


            [JsonPropertyName("file")]
            public string File { get; set; } = string.Empty;
        }
    }
}
