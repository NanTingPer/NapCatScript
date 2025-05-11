namespace NapCatScript.Core.NetWork.NetWorkModel;

[Serializable]
public class NetWorkSetConfigValue
{
    [JsonPropertyName("network")]
    public NetWorks NetWork { get; set; }

    [JsonPropertyName("musicSignUrl")]
    public string MusicSignUrl { get; set; }

    [JsonPropertyName("enableLocalFile2Url")]
    public bool EnableLocalFile2Url { get; set; }

    [JsonPropertyName("parseMultMsg")]
    public bool ParseMultMsg { get; set; }
}

[Serializable]
public class NetWorkSetConf
{
    [JsonPropertyName("config")]
    public string Config { get; set; }
}