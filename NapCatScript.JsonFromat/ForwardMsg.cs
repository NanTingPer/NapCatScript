namespace NapCatScript.JsonFromat;

public class ForwardMsg
{
    
}

/// <summary>
/// 合并转发消息
/// </summary>
public class ForwardMsgJson
{
#pragma warning disable CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
    private ForwardMsgJson(string id, MesgTo type)
#pragma warning restore CS8618 // 在退出构造函数时，不可为 null 的字段必须包含非 null 值。请考虑添加 "required" 修饰符或声明为可为 null。
    {
        switch (type) {
            case MesgTo.user:
                User_id = id;
                break;
            case MesgTo.group:
                Group_id = id;
                break;
        }
    }

    /// <summary>
    /// 创建转发消息内容只有一条的消息
    /// </summary>
    public ForwardMsgJson(string id, ForwardMsgJsonMsg content, MesgTo type) : this(id, type)
    {
        Messages = [content];
    }

    /// <summary>
    /// 创建转发消息
    /// </summary>
    public ForwardMsgJson(string id, List<ForwardMsgJsonMsg> contents, MesgTo type) : this(id, type)
    {
        Messages = contents;
    }

    /// <summary>
    /// 创建转发消息
    /// </summary>
    public ForwardMsgJson(string id, MesgTo type, params ForwardMsgJsonMsg[] contents) : this(id, type)
    {
        Messages = [];
        Messages.AddRange(contents);
    }

    /// <summary>
    /// 创建转发消息
    /// </summary>
    /// <param name="prompt">外显内容</param>
    public ForwardMsgJson(string id, List<ForwardMsgJsonMsg> contents, string prompt, MesgTo type) : this(id, type)
    {
        Messages = contents;
        Prompt = prompt;
    }

    [JsonPropertyName("user_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? User_id { get; set; } = null;

    [JsonPropertyName("group_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Group_id { get; set; } = null;

    [JsonPropertyName("messages")]
    public List<ForwardMsgJsonMsg> Messages { get; set; }

    [JsonPropertyName("news")]
    public string News { get; set; } = string.Empty;

    /// <summary>
    /// 外显内容
    /// </summary>
    [JsonPropertyName("prompt")]
    public string Prompt { get; set; } = string.Empty;

    /// <summary>
    /// 底下文本
    /// </summary>
    [JsonPropertyName("summary")]
    public string Summary { get; set; } = string.Empty;

    [JsonPropertyName("source")]
    public string Source { get; set; } = string.Empty;
}

/// <summary>
/// Message 合并转发消息的内容，是单条的
/// <para> 例: id为123456的蔚蓝说: 你好 </para>
/// </summary>
public class ForwardMsgJsonMsg
{
    public ForwardMsgJsonMsg(ForwardData data)
    {
        Data = data;
    }

    /// <summary>
    /// 用户id 111111 <para></para>
    /// <para></para> 用户昵称 蔚蓝
    /// <para></para> 说: 你好
    /// </summary>
    /// <param name="user_id"></param>
    /// <param name="nickname"></param>
    /// <param name="content"></param>
    public ForwardMsgJsonMsg(string user_id, string nickname, MsgJson content)
    {
        Data = new ForwardData(user_id, nickname, content);
    }

    [JsonPropertyName("type")]
    public string Type { get; set; } = "node";

    [JsonPropertyName("data")]
    public ForwardData Data { get; set; }
}

/// <summary>
/// 合并转发消息的实际内容，建议为单条内容
/// <para>使用了List是因为一条消息可能包含 一张图片+一个文本</para>
/// </summary>
public class ForwardData
{
    /// <summary>
    /// 空消息
    /// </summary>
    /// <param name="user_id"></param>
    /// <param name="nickname"></param>
    public ForwardData(string user_id, string nickname)
    {
        User_id = user_id;
        NickName = nickname;
        Content = [];
    }

    /// <summary>
    /// 本消息只有一种类型
    /// </summary>
    public ForwardData(string user_id, string nickname, MsgJson content) : this(user_id, nickname)
    {
        Content = [content];
    }

    /// <summary>
    /// 本消息带有多种类型
    /// </summary>
    public ForwardData(string user_id, string nickname, params MsgJson[] content) : this(user_id, nickname)
    {
        Content = [];
        Content.AddRange(content);
    }

    /// <summary>
    /// 本消息带有多种类型
    /// </summary>
    public ForwardData(string user_id, string nickname, List<MsgJson> content) : this(user_id, nickname)
    {
        Content = content;
    }

    /// <summary>
    /// 本消息昵称和user_id分别为 '匿名' '123456'
    /// </summary>
    public ForwardData(List<MsgJson> content) : this("123456", "匿名", content) { }

    /// <summary>
    /// 本消息昵称和user_id分别为 '匿名' '123456'
    /// </summary>
    public ForwardData(params MsgJson[] content) : this("123456", "匿名", content) { }

    /// <summary>
    /// 消息只有一种类型
    /// <para></para> 本消息昵称和user_id分别为 '匿名' '123456'
    /// </summary>
    public ForwardData(MsgJson content) : this("123456", "匿名", content) { }

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
    /// 本消息的内容
    /// </summary>
    [JsonPropertyName("content")]
    public List<MsgJson> Content { get; set; }
}
