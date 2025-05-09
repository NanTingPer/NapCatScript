namespace NapCatScript.Core.NetWork.NetWorkModel;

/// <summary>
/// 请求负载: config : + NewtWorkPost
/// </summary>
[Serializable]
public class NetWorkPost
{
    [JsonPropertyName("network")]
    public NetWorks NetWork { get; set; }

    [JsonPropertyName("musicSignUrl")]
    public string MusicSignUrl { get; set; } = "";

    [JsonPropertyName("enableLocalFile2Url")]
    public bool EnableLocalFile2Url { get; set; } = false;

    [JsonPropertyName("parseMultMsg")]
    public bool ParseMultMsg { get; set; } = false;
}
