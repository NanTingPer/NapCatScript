using NapCatScript.Core.JsonFormat;
using static NapCatScript.Core.JsonFormat.Msgs.JsonJson;

namespace NapCatScript.Core.JsonFormat.Msgs;

/// <summary>
/// Json卡片消息
/// </summary>
public class JsonMsg : BaseMsg
{
    public override string JsonText { get; set; }
    public override JsonElement JsonElement { get; set; }
    public override JsonDocument JsonDocument { get; set; }
    public override dynamic JsonObject { get; set; }

    public JsonMsg(string content)
    {
        JsonMsgData data = new JsonMsgData(content);
        JsonJson obj = new JsonJson(data);
        JsonText = JsonSerializer.Serialize(obj);
        JsonElement = JsonSerializer.SerializeToElement(obj);
        JsonDocument = JsonSerializer.SerializeToDocument(obj);
        JsonObject = obj;
    }
}

/// <summary>
/// Json消息的Json对象
/// </summary>
public class JsonJson : MsgJson
{
    [JsonIgnore]
    public override string JsonText { get; set; }
    public JsonJson(JsonMsgData data)
    {
        Data = data;
        JsonText = JsonSerializer.Serialize(this);
    }

    [JsonPropertyName("type")]
    public MsgType Type { get; set; } = MsgType.json;

    [JsonPropertyName("data")]
    public JsonMsgData Data { get; set; }

    //JsonClass
    public class JsonMsgData
    {
        public JsonMsgData(string content)
        {
            data = content;
        }

        [JsonPropertyName("data")]
        public string data { get; set; }
    }
}


