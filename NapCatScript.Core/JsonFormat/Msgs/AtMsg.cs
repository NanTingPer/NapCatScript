using NapCatScript.Core.JsonFormat;
using System.Text.Json.Nodes;

namespace NapCatScript.Core.JsonFormat.Msgs;

/// <summary>
/// @(At)消息
/// </summary>
public class AtMsg : BaseMsg
{
    public override string JsonText { get; set; }
    public override JsonElement JsonElement { get; set; }
    public override JsonDocument JsonDocument { get; set; }
    public override dynamic JsonObject { get; set; }
    public AtMsg(string qqid)
    {
        var jsonObject = new AtJson(qqid);
        JsonText = JsonSerializer.Serialize(jsonObject);
        JsonElement = JsonSerializer.SerializeToElement(jsonObject);
        JsonDocument = JsonSerializer.SerializeToDocument(jsonObject);
        JsonObject = jsonObject;
    }

    public AtMsg(string name, string qqid)
    {
        var jsonObject = new AtJson(name, qqid);
        JsonText = JsonSerializer.Serialize(jsonObject);
        JsonElement = JsonSerializer.SerializeToElement(jsonObject);
        JsonDocument = JsonSerializer.SerializeToDocument(jsonObject);
        JsonObject = jsonObject;
    }
}

/// <summary>
/// at消息的json
/// </summary>
public class AtJson : MsgJson
{
    [JsonIgnore]
    public override string JsonText { get; set; }
    public AtJson(string qqid)
    {
        Data = new AtMsgData(qqid);
        JsonText = JsonSerializer.Serialize(this);
    }

    public AtJson(string name, string qqid)
    {
        Data = new AtMsgData(name, qqid);
        JsonText = JsonSerializer.Serialize(this);
    }

    [JsonPropertyName("data")]
    public AtMsgData Data { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = MsgType.at.ToString();

    public class AtMsgData
    {
        public AtMsgData(string qqid)
        {
            QQ = qqid;
        }

        public AtMsgData(string name, string qqid)
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


