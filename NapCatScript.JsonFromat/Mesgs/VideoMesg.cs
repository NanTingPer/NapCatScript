using static NapCatScript.JsonFromat.Mesgs.VideoMsgJson;

namespace NapCatScript.JsonFromat.Mesgs;

/// <summary>
/// 视频消息
/// </summary>
public class VideoMesg : BaseMsg
{
    public override string JsonText { get; set; }
    public override JsonElement JsonElement { get; set; }
    public override JsonDocument JsonDocument { get; set; }
    public override dynamic JsonObject { get; set; }

    /// <param name="file">本地路径或者网络路径, file://D:/a.mp4</param>
    public VideoMesg(string file)
    {
        VideoMsgData data = new VideoMsgData(file);
        VideoMsgJson obj = new VideoMsgJson(data);
        JsonText = JsonSerializer.Serialize(obj);
        JsonElement = JsonSerializer.SerializeToElement(obj);
        JsonDocument = JsonSerializer.SerializeToDocument(obj);
        JsonObject = obj;
    }
}

/// <summary>
/// 视频消息的Json对象
/// </summary>
public class VideoMsgJson : MsgJson
{
    public VideoMsgJson(VideoMsgData data)
    {
        Data = data;
    }

    [JsonPropertyName("type")]
    public MsgType Type { get; set; } = MsgType.video;//语音消息

    [JsonPropertyName("data")]
    public VideoMsgData Data { get; set; }

    //JsonClass

    public class VideoMsgData
    {
        public VideoMsgData(string content)
        {
            file = content;
        }

        [JsonPropertyName("file")]
        public string file { get; set; }
    }


}

