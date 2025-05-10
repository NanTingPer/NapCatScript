namespace NapCatScript.Core.NetWork.NetWorkModel;

//"name": "HTTPCLIENT",
//"enable": false,
//"url": "http://localhost:8080",
//"messagePostFormat": "array",
//"reportSelfMessage": false,
//"token": "",
//"debug": false
[Serializable]
public class HttpClient
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = "HttpClient";

    [JsonPropertyName("enable")]
    public bool Enable { get; set; } = false;
    
    [JsonPropertyName("url")]
    public string Url { get; set; } = "http://127.0.0.1:8080";

    [JsonPropertyName("messagePostFormat")]
    public string MessagePostFormat { get; set; } = "string";//array

    [JsonPropertyName("reportSelfMessage")]
    public bool ReportSelfMessage { get; set; } = false;

    [JsonPropertyName("token")]
    public string ToKen { get; set; } = "";

    [JsonPropertyName("debug")]
    public bool Debug { get; set; } = false;
}
