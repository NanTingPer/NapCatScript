using System.Text;

namespace NapCatScript.Core.JsonFormat.Msgs;
/// <summary>
/// 构建群聊文本消息与私聊文本消息
/// </summary>
public class TextMesg
{
    private Root MesgObject { get; set; }
    private JsonDocument MesgJson { get; set; }
    public string MesgString { get; private set; }

    /// <summary>
    /// 构建 使用MessagesJsonObject获取
    /// </summary>
    /// <param name="user_id"> 用户id </param>
    /// <param name="text"> 要发送的消息 </param>
    public TextMesg(string user_id, MsgTo mestype,string text)
    {
        Data data = new Data(text);
        Message message = new Message(data);
        MesgObject = new Root(user_id, new List<Message>() { message });
        MesgJson = JsonSerializer.SerializeToDocument(MesgObject);
        MesgString = JsonSerializer.Serialize(MesgObject);
        if (mestype == MsgTo.group) {
            string[] strings = MesgString.Split("user_id");
            StringBuilder sbuilder = new StringBuilder();
            sbuilder.Append(strings[0]);
            sbuilder.Append("group_id");
            for (int i = 1; i < strings.Length; i++) {
                sbuilder.Append(strings[i]);
            }
            MesgString = sbuilder.ToString();
        }
    }

    public class Root
    {
        public Root(string user_id, List<Message> message)
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
        public Message(Data data, string type = "text")
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
