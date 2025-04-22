namespace NapCatScript.Core.JsonFormat.JsonModel;

/// <summary>
/// 获取推荐群聊卡片Json
/// </summary>
public class ArkShareGroup(string group_id) : RequestJson //获取推荐群聊卡片Json
{
    public override string JsonText { get; set; } = JsonSerializer.Serialize(new Root(group_id));

    public class Root(string group_id)
    {
        [JsonPropertyName("group_id")]
        public string Group_id { get; set; } = group_id;
    }
}

/// <summary>
/// ArkShareGroupReturn的返回
/// </summary>
public class ArkShareGroupReturn
{
    /// <summary>
    /// 卡片json
    /// </summary>
    [JsonPropertyName("data")]
    public string Data { get; set; }

    [JsonPropertyName("echo")]
    public string Echo { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("retcode")]
    public double Retcode { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("wording")]
    public string Wording { get; set; }
}
