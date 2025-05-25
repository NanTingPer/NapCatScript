namespace NapCatScript.Core.JsonFormat.Msgs;
/// <summary>
/// 二级合并转发消息的Json
/// </summary>
public class TwoForwardJson : MsgJson
{
    public TwoForwardJson(TwoForawrdData data)
    {
        Data = data;
    }

    public TwoForwardJson(long user_id, string nickname, List<MsgJson> contents) :
        this (user_id.ToString(), nickname, contents)
    {
        
    }
    
    public TwoForwardJson(string user_id, string nickname, List<MsgJson> contents)
    {
        Data = new TwoForawrdData(user_id, nickname, contents);
    }

    public TwoForwardJson(long user_id, string nickname, MsgJson contents) : 
        this(user_id.ToString(), nickname, contents)
    {}
    
    public TwoForwardJson(string user_id, string nickname, MsgJson contents)
    {
        Data = new TwoForawrdData(user_id, nickname, [contents]);
    }


    public TwoForwardJson(MsgJson content)
    {
        Data = new TwoForawrdData(content);
    }

    /// <summary>
    /// 类型，固定值为node
    /// </summary>
    [JsonPropertyName("type")]
    public string Type { get; set; } = "node";

    /// <summary>
    /// 内容
    /// </summary>
    [JsonPropertyName("data")]
    public TwoForawrdData Data { get; set; }
}

/// <summary>
/// 二级合并转发消息Json的Data
/// </summary>
public class TwoForawrdData
{
    public void SetDefualtValue()
    {
        User_id ??= "123456";
        NickName ??= "匿名";
        Content ??= [];
    }

    /// <summary>
    /// 构建空二级转发Data
    /// </summary>
    /// <param name="user_id"></param>
    public TwoForawrdData(string user_id)
    {
        User_id = user_id;
        SetDefualtValue();
    }

    /// <summary>
    /// 构建空二级转发Data
    /// </summary>
    /// <param name="user_id"> 发起ID </param>
    /// <param name="nickname"> 发起Name </param>
    public TwoForawrdData(string user_id, string nickname)
    {
        User_id = user_id;
        NickName = nickname;
        SetDefualtValue();
    }

    /// <summary>
    /// 构建带内容带昵称的二级转发内容
    /// </summary>
    /// <param name="user_id"> 发起ID </param>
    /// <param name="nickname"> 发起名称 </param>
    /// <param name="msgJsons"> 消息内容 </param>
    public TwoForawrdData(string user_id, string nickname, List<MsgJson> msgJsons)
    {
        User_id = user_id;
        NickName = nickname;
        Content = msgJsons;
    }

    /// <summary>
    /// 构建单个内容的二层合并转发消息，id与name会为默认值
    /// </summary>
    public TwoForawrdData(MsgJson content)
    {
        Content = [content];
        SetDefualtValue();
    }

    /// <summary>
    /// 消息发起者的ID
    /// </summary>
    [JsonPropertyName("user_id")]
    public string? User_id { get; set; }

    /// <summary>
    /// 消息发起者的昵称
    /// </summary>

    [JsonPropertyName("nickname")]
    public string? NickName { get; set; }

    /// <summary>
    /// 此合并转发消息的内容
    /// </summary>
    [JsonPropertyName("content")]
    public List<MsgJson>? Content { get; set; }

    /// <summary>
    /// 外显内容，可选
    /// </summary>
    [JsonPropertyName("news")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<string>? News { get; set; } = null;

    /// <summary>
    /// 外显，可选
    /// </summary>
    [JsonPropertyName("prompt")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Prompt { get; set; } = null;

    /// <summary>
    /// 底下文本，可选
    /// </summary>
    [JsonPropertyName("summary")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Summary { get; set; } = null;

    /// <summary>
    /// 标题，可选
    /// </summary>
    [JsonPropertyName("source")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Source { get; set; } = null;
}
