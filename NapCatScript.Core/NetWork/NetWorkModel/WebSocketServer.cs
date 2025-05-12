namespace NapCatScript.Core.NetWork.NetWorkModel;
//"name": "TestNet",
//"enable": true,
//"host": "127.0.0.1",
//"port": 9999,
//"messagePostFormat": "string",
//"reportSelfMessage": true,
//"token": "",
//"enableForcePushEvent": true,
//"debug": false,
//"heartInterval": 5000
[Serializable]
public class WebSocketServer
{
    /// <summary>
    /// 名称
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = "WebSocketServer";

    /// <summary>
    /// 是否启用
    /// </summary>
    [JsonPropertyName("enable")]
    public bool Enable { get; set; } = false;

    /// <summary>
    /// 主机
    /// </summary>
    [JsonPropertyName("host")]
    public string Host { get; set; } = "127.0.0.1";

    /// <summary>
    /// 端口
    /// </summary>
    [JsonPropertyName("port")]
    public int Port { get; set; } = 9999;

    /// <summary>
    /// 上报类型
    /// </summary>
    [JsonPropertyName("messagePostFormat")]
    public string MessagePostFormat { get; set; } = "string";//array

    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("reportSelfMessage")]
    public bool ReportSelfMessage { get; set; } = false;

    [JsonPropertyName("token")]
    public string Token { get; set; } = "";

    [JsonPropertyName("enableForcePushEvent")]
    public bool EnableForcePushEvent { get; set; }

    [JsonPropertyName("debug")]
    public bool Debug { get; set; } = false;
    [JsonPropertyName("heartInterval")]
    public int HeartInterval { get; set; } = 5000;

}
