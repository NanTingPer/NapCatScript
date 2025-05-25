namespace NapCatScript.Core.JsonFormat;

public class ForwardMsg
{
    
}

/// <summary>
/// 合并转发消息
/// </summary>
public class ForwardMsgJson
{
    public ForwardMsgJson()
    {
        
    }
    
    private ForwardMsgJson(string id, MsgTo type)
    {
        switch (type) {
            case MsgTo.user:
                User_id = id;
                break;
            case MsgTo.group:
                Group_id = id;
                break;
        }
        Messages = new List<ForawrdMsgJsonMessage>();
    }

    /// <summary>
    /// 创建转发消息内容只有一条的消息
    /// </summary>
    /// <param name="id"> 群聊id / 用户id </param>
    public ForwardMsgJson(string id, ForwardData/*ForwardMsgJsonMsg*/ content, MsgTo type) : this(id, type)
    {
        Messages.Add(new ForawrdMsgJsonMessage(content));
    }

    /// <summary>
    /// 创建转发消息
    /// </summary>
    /// <param name="id"> 群聊id / 用户id </param>
    public ForwardMsgJson(string id, List<ForwardData/*ForwardMsgJsonMsg*/> contents, MsgTo type) : this(id, type)
    {
        //Messages.Data = contents;
        foreach (var item in contents) {
            Messages.Add(new ForawrdMsgJsonMessage(item));
        }
    }

    /// <summary>
    /// 创建转发消息
    /// </summary>
    /// <param name="id"> 群聊id / 用户id </param>
    public ForwardMsgJson(string id, MsgTo type, params ForwardData/*ForwardMsgJsonMsg*/[] contents) : this(id, type)
    {
        //Messages.Data.AddRange(contents);
        foreach (var item in contents) {
            Messages.Add(new ForawrdMsgJsonMessage(item));
        }
    }

    /// <summary>
    /// 创建转发消息
    /// </summary>
    /// <param name="id"> 群聊id / 用户id </param>
    /// <param name="prompt">外显内容</param>
    public ForwardMsgJson(string id, List<ForwardData/*ForwardMsgJsonMsg*/> contents, string prompt, MsgTo type) : this(id, type)
    {
        //Messages.Data = contents;
        foreach (var item in contents) {
            Messages.Add(new ForawrdMsgJsonMessage(item));
        }
        Prompt = prompt;
    }

    [JsonPropertyName("user_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? User_id { get; set; } = null;

    [JsonPropertyName("group_id")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Group_id { get; set; } = null;

    [JsonPropertyName("messages")]
    public List<ForawrdMsgJsonMessage> Messages { get; set; }

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

public class ForawrdMsgJsonMessage
{
    public ForawrdMsgJsonMessage(ForwardData/*ForwardMsgJsonMsg*//*List<ForwardMsgJsonMsg>*/ data)
    {
        Data = data;
    }

    [JsonPropertyName("type")]
    public string Type { get; set; } = "node";

    [JsonPropertyName("data")]
    public ForwardData Data { get; set; }
    //public ForwardMsgJsonMsg Data { get; set; }
    //public List<ForwardMsgJsonMsg> Data { get; set; }
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
    /// <param name="user_id"> 发起者的id </param>
    /// <param name="nickname"> 发起者的昵称 </param>
    public ForwardData(string user_id, string nickname)
    {
        User_id = user_id;
        NickName = nickname;
        Content = [];
    }

    /// <summary>
    /// 本消息只有一种类型
    /// </summary>
    /// <param name="content"> 发起者发送的内容 </param>
    /// <param name="user_id"> 发起者的id </param>
    /// <param name="nickname"> 发起者的昵称 </param>
    public ForwardData(string user_id, string nickname, MsgJson content) : this(user_id, nickname)
    {
        Content = [content];
    }
    
    public ForwardData(long user_id, string nickname, MsgJson content) : this(user_id.ToString(), nickname)
    {
        Content = [content];
    }

    /// <summary>
    /// 本消息带有多种类型
    /// </summary>
    /// <param name="user_id"> 发起者的id </param>
    /// <param name="nickname"> 发起者的昵称 </param>
    public ForwardData(string user_id, string nickname, params MsgJson[] content) : this(user_id, nickname)
    {
        Content = [];
        Content.AddRange(content);
    }

    /// <summary>
    /// 本消息带有多种类型
    /// </summary>
    /// <param name="user_id"> 发起者的id </param>
    /// <param name="nickname"> 发起者的昵称 </param>
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


#region Delete 保留
/*
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
    /// <param name="user_id"> 发起者id </param>
    /// <param name="nickname"> 发起者昵称 </param>
    /// <param name="content"> 发起者发送的内容 </param>
    public ForwardMsgJsonMsg(string user_id, string nickname, MsgJson content)
    {
        Data = new ForwardData(user_id, nickname, content);
    }

    [JsonPropertyName("type")]
    public string Type { get; set; } = "node";

    [JsonPropertyName("data")]
    public ForwardData Data { get; set; }
}
*/
#endregion