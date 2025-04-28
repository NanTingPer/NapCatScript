namespace NapCatScript.Core.JsonFormat;

/// <summary>
/// 最终消息类
/// </summary>
[Obsolete(message: "改用MsgJson", true)]
public class MesgHandle : BaseMsg
{
    public override string JsonText { get; set; }
    public override JsonElement JsonElement { get; set; }
    public override JsonDocument JsonDocument { get; set; }
    public override dynamic JsonObject { get; set; }

    /// <summary>
    /// 构建 使用MessagesJsonObject获取
    /// </summary>
    /// <param name="user_id"> 用户id </param>
    /// <param name="text"> 要发送的消息 </param>
    public MesgHandle(string user_id, MsgTo mestype, params BaseMsg[] mesgs)
    {
        List<string> contents = [];
        foreach (var item in mesgs) {
            contents.Add(item.ToString());
        }

        Root root = new Root(user_id, contents, mestype);
        JsonText = JsonSerializer.Serialize(root);
        JsonElement = JsonSerializer.SerializeToElement(root);
        JsonDocument = JsonSerializer.SerializeToDocument(root);
        JsonObject = root;
    }


    public class Root
    {
        public Root(string user_id, List<string> message, MsgTo sendTo)
        {
            switch (sendTo) {
                case MsgTo.group:
                    Group_id = user_id;
                    Message_Type = "group";
                    break;
                case MsgTo.user:
                    User_id = user_id;
                    Message_Type = "private";
                    break;
            }
            Message_Type ??= "private";
            Message = message;
        }

        [JsonPropertyName("message_type")]
        public string Message_Type { get; set; }

        [JsonPropertyName("user_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? User_id { get; set; } = null;

        [JsonPropertyName("group_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Group_id { get; set; } = null;

        [JsonPropertyName("message")]
        public List<string> Message { get; set; }
    }

}
