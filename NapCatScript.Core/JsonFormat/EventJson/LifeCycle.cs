namespace NapCatScript.Core.JsonFormat.EventJson;

/// <summary>
/// 心跳事件
/// </summary>
public sealed class LifeCycle : Event
{

    [JsonPropertyName("meta_event_type")]
    public string MetaEventType { get; set; }

    /// <summary>
    /// 状态对象
    /// </summary>
    [JsonPropertyName("status")]
    public LifeCycleStatus Status { get; set; }

    /// <summary>
    /// 间隔
    /// </summary>
    [JsonPropertyName("interval")]
    public int Interval { get; set; }

    [JsonPropertyName("time")]
    public override long Time { get; set; }

    [JsonPropertyName("self_id")]
    public override long SelfId { get; set; }

    [JsonPropertyName("post_type")]
    public override string PostType { get; set; }
}
public sealed class LifeCycleStatus
{
    /// <summary>
    /// 在线状态
    /// </summary>
    [JsonPropertyName("online")]
    public bool Online { get; set; }

    [JsonPropertyName("good")]
    public bool Good { get; set; }
}
