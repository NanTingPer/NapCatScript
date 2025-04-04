using static NapCatScript.JsonFromat.Mesgs.TextMsgJson;

namespace NapCatScript.JsonFromat.Mesgs;

public class TextMsgBak : BaseMsg
{
    public override string JsonText { get; set; }
    public override JsonElement JsonElement { get; set; }
    public override JsonDocument JsonDocument { get; set; }
    public override dynamic JsonObject { get; set; }

    public TextMsgBak(string content)
    {
        var jsonObject = new TextMsgJson(new TextMsgData(content));
        JsonText = JsonSerializer.Serialize(jsonObject);
        JsonElement = JsonSerializer.SerializeToElement(jsonObject);
        JsonDocument = JsonSerializer.SerializeToDocument(jsonObject);
        JsonObject = jsonObject;
    }

}

/// <summary>
/// 文本消息的Json对象
/// </summary>
public class TextMsgJson : MsgJson
{
    public TextMsgJson(TextMsgData data, string type = "text")
    {
        Data = data;
        Type = type;
    }

    [JsonPropertyName("type")]
    public string Type { get; set; } = "text";

    [JsonPropertyName("data")]
    public TextMsgData Data { get; set; } = new TextMsgData();

    public class TextMsgData
    {
        public TextMsgData(string text = "unll")
        {
            Text = text;
        }

        [JsonPropertyName("text")]
        public string Text { get; set; } = "null";
    }

}

