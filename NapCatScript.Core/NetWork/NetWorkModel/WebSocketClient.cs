namespace NapCatScript.Core.NetWork.NetWorkModel;

//"name": "WENSOCKETCLIENT",
//"enable": false,
//"url": "ws://localhost:8082",
//"messagePostFormat": "array",
//"reportSelfMessage": false,
//"reconnectInterval": 30000,
//"token": "",
//"debug": false,
//"heartInterval": 30000
[Serializable]
public class WebSocketClient
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "WebSocketClient";

    [JsonPropertyName("enable")]
    public bool Enable { get; set; } = false;

    [JsonPropertyName("url")]
    public string Url { get; set; } = "ws://127.0.0.1:8082";

    [JsonPropertyName("messagePostFormat")]
    public string MessagePostFormat { get; set; } = "string";//array

    [JsonPropertyName("reportSelfMessage")]
    public bool ReportSelfMessage { get; set; } = false;

    [JsonPropertyName("reconnectInterval")]
    public int ReconnectInterval { get; set; } = 30000;

    [JsonPropertyName("token")]
    public string Token { get; set; } = "";

    [JsonPropertyName("debug")]
    public bool Debug { get; set; } = false;

    [JsonPropertyName("heartInterval")]
    public int HeartInterval { get; set; } = 30000;
}
