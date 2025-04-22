using System.IO;
using System.Text;

namespace NapCatScript.Core.JsonFormat.Msgs;
/// <summary>
/// 图片消息 Json构建
/// <para> 使用MassageJsonObject字段取出string值 </para>
/// </summary>
public class ImageMesg
{
    private Root MesgObject { get; set; }
    private JsonDocument MesgJson { get; set; }
    public string MesgString { get; private set; }

    /// <summary>
    /// 构建
    /// </summary>
    /// <param name="user_id"> 目标ID </param>
    /// <param name="type"> 类型 默认来就行了 </param>
    /// <param name="fileUrl"> 文件链接 / base64 </param>
    public ImageMesg(string user_id, MsgTo mestype, params string[] fileUrl)
    {
        List<Message> mesList = new List<Message>();
        MsgType type = MsgType.image;
        foreach (var url in fileUrl) {
            if (File.Exists(url)) {
                mesList.Add(new Message(type, new Data(Utils.ImageToBase64(url))));
            }
        }

        MesgObject = new Root(user_id, mesList);
        MesgJson = JsonSerializer.SerializeToDocument(MesgObject);
        MesgString = JsonSerializer.Serialize(MesgObject);
        if(mestype == MsgTo.group) {
            string[] strings = MesgString.Split("user_id");
            StringBuilder sbuilder = new StringBuilder();
            sbuilder.Append(strings[0]);
            sbuilder.Append("group_id");
            for(int i = 1; i < strings.Length; i++) {
                sbuilder.Append(strings[i]);
            }
            MesgString = sbuilder.ToString();
        }
    }

    /// <summary>
    /// 目标ID
    /// </summary>
    public class Root
    {
        public Root(string user_id, List<Message> messages)
        {
            User_id = user_id;
            Messages = messages;
        }
        [JsonPropertyName("user_id")]
        public string User_id { get; set; } = string.Empty;

        [JsonPropertyName("message")]
        public List<Message> Messages { get; set; } = new List<Message>();
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
