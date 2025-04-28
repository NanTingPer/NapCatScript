namespace NapCatScript.Core.JsonFormat.Msgs;

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


