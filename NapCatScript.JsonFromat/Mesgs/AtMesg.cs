using System.Text.Json.Nodes;

namespace NapCatScript.JsonFromat.Mesgs;

/// <summary>
/// @(At)消息
/// </summary>
public class AtMesg : BaseMesg
{
    public override string JsonText { get; set; }
    public override JsonElement JsonElement { get; set; }
    public override JsonDocument JsonDocument { get; set; }
    public override dynamic JsonObject { get; set; }
    public AtMesg(string qqid)
    {
        var jsonObject = new Root(qqid);
        JsonText = JsonSerializer.Serialize(jsonObject);
        JsonElement = JsonSerializer.SerializeToElement(jsonObject);
        JsonDocument = JsonSerializer.SerializeToDocument(jsonObject);
        JsonObject = jsonObject;
    }

    public AtMesg(string name, string qqid)
    {
        var jsonObject = new Root(name, qqid);
        JsonText = JsonSerializer.Serialize(jsonObject);
        JsonElement = JsonSerializer.SerializeToElement(jsonObject);
        JsonDocument = JsonSerializer.SerializeToDocument(jsonObject);
        JsonObject = jsonObject;
    }

    private class Root
    {
        public Root(string qqid)
        {
            Message = new Data(qqid);
        }

        public Root(string name, string qqid)
        {
            Message = new Data(name, qqid);
        }

        [JsonPropertyName("message")]
        public Data Message { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = MsgType.at.ToString();
    }

    private class Data
    {
        public Data(string qqid)
        {
            QQ = qqid;
        }

        public Data(string name, string qqid)
        {
            Name = name;
            QQ = qqid;
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("name")]
        public string? Name { get; set; } = null;

        [JsonPropertyName("qq")]
        public string QQ { get; set; }
    }
}