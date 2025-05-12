namespace NapCatScript.Core.NetWork.NetWorkModel;

[Serializable]
public class HttpServer
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "HttpServer";
    [JsonPropertyName("enable")]
    public bool Enable { get; set; } = false;
    [JsonPropertyName("port")]
    public int Port { get; set; } = 9998;
    [JsonPropertyName("host")]
    public string Host { get; set; } = "127.0.0.1";
    [JsonPropertyName("enableCors")]
    public bool EnableCors { get; set; } = false;
    [JsonPropertyName("enableWebsocket")]
    public bool EnableWebSocket { get; set; } = false;
    [JsonPropertyName("messagePostFormat")]
    public string MessagePostFormat { get; set; } = "string"; //array
    [JsonPropertyName("token")]
    public string Token { get; set; } = "";
    [JsonPropertyName("debug")]
    public bool Debug { get; set; } = false;
}
