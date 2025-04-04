using static NapCatScript.JsonFromat.Mesgs.JsonMsgJson;

namespace NapCatScript.JsonFromat.Mesgs;

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
        JsonMsgJson obj = new JsonMsgJson(data);
        JsonText = JsonSerializer.Serialize(obj);
        JsonElement = JsonSerializer.SerializeToElement(obj);
        JsonDocument = JsonSerializer.SerializeToDocument(obj);
        JsonObject = obj;
    }
}

/// <summary>
/// Json消息的Json对象
/// </summary>
public class JsonMsgJson : MsgJson
{
    public JsonMsgJson(JsonMsgData data)
    {
        Data = data;
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


