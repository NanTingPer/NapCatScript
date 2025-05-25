namespace NapCatScript.Core.JsonFormat.Msgs;

/// <summary>
/// MarkDown消息的Json对象
/// </summary>
public class MarkDownJson : MsgJson
{
    public MarkDownJson(MarkDownJsonData data)
    {
        Data = data;
    }

    public MarkDownJson(string content)
    {
        Data = new MarkDownJsonData(content);
    }


    [JsonPropertyName("type")]
    public string Type { get; set; } = "markdown";

    [JsonPropertyName("data")]
    public MarkDownJsonData Data { get; set; } = new MarkDownJsonData();

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

