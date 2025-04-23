namespace NapCatScript.Core.JsonFormat.EventJson;


[JsonDerivedType(typeof(LifeCycle))]
public abstract class Event
{
    public Event()
    {
        EventType = this.GetType();
        PostTypeEnum = PostType switch
        {
            "message" => EventJson.PostType.message,
            "notice" => EventJson.PostType.notice,
            "request" => EventJson.PostType.request,
            "meta_event" => EventJson.PostType.meta_event,
            _ => EventJson.PostType.Null,
        };
    }

    /// <summary>
    /// 本事件的实际类型，可用于类型转换
    /// </summary>
    [JsonIgnore]
    public virtual Type EventType { get; init; }

    [JsonPropertyName("time")]
    public abstract long Time { get; set; }

    [JsonPropertyName("self_id")]
    public abstract long SelfId { get; set; }

    [JsonPropertyName("post_type")]
    public abstract string PostType { get; set; }

    [JsonIgnore]
    public PostType PostTypeEnum { get; init; }
}

public enum PostType
{
    /// <summary>
    /// 消息事件
    /// </summary>
    message,

    /// <summary>
    /// 通知事件
    /// </summary>
    notice,

    /// <summary>
    /// 请求事件
    /// </summary>
    request,

    /// <summary>
    /// 元事件
    /// </summary>
    meta_event,

    Null
}
