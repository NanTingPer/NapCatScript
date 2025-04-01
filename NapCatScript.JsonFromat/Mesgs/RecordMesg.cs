namespace NapCatScript.JsonFromat.Mesgs;

/// <summary>
/// 语音消息
/// </summary>
public class RecordMesg : BaseMesg
{
    public override string JsonText { get; set; }

    /// <param name="content">本地路径或者网络路径, file://D:/a.mp3</param>
    public RecordMesg(string content)
    {
        Data data = new Data(content);
        JsonClass obj = new JsonClass(data);
        JsonText = JsonSerializer.Serialize(obj);
    }

    public class JsonClass
    {
        public JsonClass(Data data)
        {
            Data = data;
        }

        [JsonPropertyName("type")]
        public MsgType Type { get; set; } = MsgType.record;//语音消息

        [JsonPropertyName("data")]
        public Data Data { get; set; }

        //JsonClass
    }

    public class Data
    {
        public Data(string content)
        {
            file = content;
        }

        [JsonPropertyName("file")]
        public string file { get; set; }
    }
}
