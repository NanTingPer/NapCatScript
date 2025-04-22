namespace NapCatScript.Core.JsonFormat.EventJson;

/// <summary>
/// ws心跳包
/// </summary>
public class LifeCycle
{
    [JsonPropertyName("time")]
    public string Time { get; set; }

    [JsonPropertyName("self_id")]
    public string SelfId { get; set; }

    [JsonPropertyName("post_type")]
    public string PostType { get; set; }

    [JsonPropertyName("meta_event_type")]
    public string MetaEventType { get; set; }

    [JsonPropertyName("sub_type")]
    public string SubType { get; set; }
}
