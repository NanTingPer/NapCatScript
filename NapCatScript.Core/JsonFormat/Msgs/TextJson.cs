namespace NapCatScript.Core.JsonFormat.Msgs;

/// <summary>
/// 文本消息的Json对象
/// </summary>
public class TextJson : MsgJson
{
    #region ctor

    [JsonConstructor]
    private TextJson()
    {
        
    }
    
    public TextJson(TextMsgData data, string type = "text")
    {
        Data = data;
        Type = type;
    }

    public TextJson(string data, string type = "text")
    {
        Data = new TextMsgData(data);
        Type = type;
    }

    #endregion
    
    [JsonPropertyName("type")]
    public string Type { get; set; } = "text";

    [JsonPropertyName("data")]
    public TextMsgData Data { get; set; } = new TextMsgData();

    public class TextMsgData
    {
        [JsonConstructor]
        private TextMsgData() { }
        
        public TextMsgData(string text = "unll")
        {
            Text = text;
        }

        [JsonPropertyName("text")]
        public string Text { get; set; } = "null";
    }

}

