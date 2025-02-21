using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JsonFromat.Mesgs
{
    /// <summary>
    /// 构建群聊文本消息与私聊文本消息
    /// </summary>
    public class TextMesg
    {
        public Root MesgObject { get; private set; }
        public JsonDocument MesgJson { get; private set; }

        /// <summary>
        /// 构建 使用MessagesJsonObject获取
        /// </summary>
        /// <param name="user_id"> 用户id </param>
        /// <param name="text"> 要发送的消息 </param>
        public TextMesg(string user_id,string text) 
        {
            Data data = new Data(text);
            Message message = new Message(data);
            MesgObject = new Root(user_id, new List<Message>() { message });
            MesgJson = JsonSerializer.SerializeToDocument(MesgObject);
        }

        public class Root
        {
            public Root(string user_id,List<Message> message) 
            { 
                User_id = user_id;
                Message = message;
            }

            [JsonPropertyName("user_id")]
            public string User_id { get; set; }

            [JsonPropertyName("message")]
            public List<Message> Message { get; set; }
        }

        public class Message
        {
            public Message(Data data,string type = "text") 
            {
                Data = data;
                Type = type;
            }

            [JsonPropertyName("type")]
            public string Type { get; set; } = "text";

            [JsonPropertyName("data")]
            public Data Data { get; set; } = new Data();
        }

        public class Data
        {
            public Data(string text = "unll") 
            { 
                Text = text;
            }

            [JsonPropertyName("text")]
            public string Text { get; set; } = "null";
        }
    }
}
