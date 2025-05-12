using System.Net.Http;

namespace NapCatScript.Core.NetWork.NetWorkModel;

[Serializable]
public class NetWorks
{
    [JsonPropertyName("httpServers")]
    public List<HttpServer> HttpServers { get; set; } = [];

    [JsonPropertyName("httpSseServers")]
    public List<HttpSseServer> HttpSseServers { get; set; } = [];

    [JsonPropertyName("httpClients")]
    public List<HttpClient> HttpClients { get; set; } = [];

    [JsonPropertyName("websocketServers")]
    public List<WebSocketServer> WebSocketServers { get; set; } = [];

    [JsonPropertyName("websocketClients")]
    public List<WebSocketClient> WebSocketClients { get; set; } = [];

    [JsonPropertyName("plugins")]
    public List<string> Plugins { get; set; } = [];
}
