using static NapCatScript.JsonFromat.Msgs.RecordJson;

namespace NapCatScript.JsonFromat.Msgs;

/// <summary>
/// 语音消息
/// </summary>
public class RecordMsg : BaseMsg
{
    public override string JsonText { get; set; }
    public override JsonElement JsonElement { get; set; }
    public override JsonDocument JsonDocument { get; set; }
    public override dynamic JsonObject { get; set; }

    /// <param name="content">本地路径或者网络路径, file://D:/a.mp3</param>
    public RecordMsg(string content)
    {
        RecordMsgData data = new RecordMsgData(content);
        RecordJson obj = new RecordJson(data);
        JsonText = JsonSerializer.Serialize(obj);
        JsonElement = JsonSerializer.SerializeToElement(obj);
        JsonDocument = JsonSerializer.SerializeToDocument(obj);
        JsonObject = obj;
    }
}

public class RecordJson : MsgJson
{
    public RecordJson(RecordMsgData data)
    {
        Data = data;
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


