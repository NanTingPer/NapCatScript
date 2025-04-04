namespace NapCatScript.JsonFromat;

public class ForwardMsg
{
    
}

public class ForwardMsgJson
{
    [JsonPropertyName("user_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? User_id { get; set; } = null;

    [JsonPropertyName("group_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Group_id { get; set; } = null;

    [JsonPropertyName("messages")]
    public List<Message> Messages { get; set; }
}

public class Message
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = "node";

    
}

public class Data
{
    /// <summary>
    /// 发送本条消息的用户id
    /// </summary>
    [JsonPropertyName("user_id")]
    public string User_id { get; set; }

    /// <summary>
    /// 显示昵称
    /// </summary>
    [JsonPropertyName("nickname")]
    public string NickName { get; set; }

    /// <summary>
    /// 内容
    /// <para>文本消息<see cref="Mesgs.TextMsgJson"/> </para>
    /// <para>表情消息<see cref="未定义"/></para>
    /// <para>图片消息<see cref="未定义"/></para>
    /// <para>回复消息<see cref="未定义"/></para>
    /// <para>Json消息<see cref="Mesgs.JsonMsgJson"/></para>
    /// <para>视频消息<see cref="Mesgs.VideoMsgJson"/></para>
    /// <para>文件消息<see cref="未定义"/></para>
    /// <para>markdown消息<see cref="未定义"/></para>
    /// <para>发送forward<see cref="未定义"/></para>
    /// <para>二级合并转发消息<see cref="未定义"/></para>
    /// </summary>
    [JsonPropertyName("content")]
    public List<MsgJson> Content { get; set; }
}
