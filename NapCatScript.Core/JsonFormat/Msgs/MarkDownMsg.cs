using NapCatScript.Core.JsonFormat;
using static NapCatScript.Core.JsonFormat.Msgs.MarkDownJson;

namespace NapCatScript.Core.JsonFormat.Msgs;

public class MarkDownMsg : BaseMsg
{
    public override string JsonText { get; set; }
    public override JsonElement JsonElement { get; set; }
    public override JsonDocument JsonDocument { get; set; }
    public override dynamic JsonObject { get; set; }

    public MarkDownMsg(string content)
    {
        var jsonObject = new MarkDownJson(new MarkDownJsonData(content));
        JsonText = JsonSerializer.Serialize(jsonObject);
        JsonElement = JsonSerializer.SerializeToElement(jsonObject);
        JsonDocument = JsonSerializer.SerializeToDocument(jsonObject);
        JsonObject = jsonObject;
    }

}

/// <summary>
/// 文本消息的Json对象
/// </summary>
public class MarkDownJson : MsgJson
{
    public MarkDownJson(MarkDownJsonData data)
    {
        Data = data;
        JsonText = JsonSerializer.Serialize(this);
    }

    public MarkDownJson(string content)
    {
        Data = new MarkDownJsonData(content);
        JsonText = JsonSerializer.Serialize(this);
    }


    [JsonPropertyName("type")]
    public string Type { get; set; } = "markdown";

    [JsonPropertyName("data")]
    public MarkDownJsonData Data { get; set; } = new MarkDownJsonData();

    [JsonIgnore]
    public override string JsonText { get; set; }

    public class MarkDownJsonData
    {
        public MarkDownJsonData(string content = "unll")
        {
            Content = content;
        }

        [JsonPropertyName("content")]
        public string Content { get; set; } = "null";
    }

}

