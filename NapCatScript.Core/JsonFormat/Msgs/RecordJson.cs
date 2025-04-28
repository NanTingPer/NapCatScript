namespace NapCatScript.Core.JsonFormat.Msgs;

public class RecordJson : MsgJson
{
    public RecordJson(RecordMsgData data)
    {
        Data = data;
        JsonText = JsonSerializer.Serialize(this);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="filePath">本地路径或者网络路径, file://D:/a.mp3</param>
    public RecordJson(string filePath)
    {
        Data = new RecordMsgData(filePath);
        JsonText = JsonSerializer.Serialize(this);
    }

    [JsonPropertyName("type")]
    public MsgType Type { get; set; } = MsgType.record;//语音消息

    [JsonPropertyName("data")]
    public RecordMsgData Data { get; set; }
    public override string JsonText { get; set; }

    public class RecordMsgData
    {
        public RecordMsgData(string content)
        {
            file = content;
        }

        [JsonPropertyName("file")]
        public string file { get; set; }
    }

    //JsonClass
}


