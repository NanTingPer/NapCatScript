using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonFromat.Mesgs
{
    /// <summary>
    /// 图片消息 Json构建
    /// <para> 使用MassageJsonObject字段取出string值 </para>
    /// </summary>
    public class ImageMesg
    {
        public Root MesgObject { get; private set; }
        public JsonDocument MesgJson { get; private set; }

        /// <summary>
        /// 构建
        /// </summary>
        /// <param name="user_id"> 目标ID </param>
        /// <param name="type"> 类型 默认来就行了 </param>
        /// <param name="fileUrl"> 文件链接 / base64 </param>
        public ImageMesg(string user_id, MsgType type = MsgType.image ,params string[] fileUrl) 
        { 
            List<Message> mesList = new List<Message>();

            foreach (var url in fileUrl)
            {
                if(File.Exists(url))
                {
                    mesList.Add(new Message(type, new Data(Utils.ImageToBase64(url))));
                }
            }

            MesgObject = new Root(user_id, mesList);
            MesgJson = JsonSerializer.SerializeToDocument(MesgObject);
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
            public Message(MsgType type, Data data) 
            { 
                Data = data;
                Type = type.ToString();
            }

            [JsonPropertyName("type")]
            public string Type { get; set; } = MsgType.image.ToString();

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
