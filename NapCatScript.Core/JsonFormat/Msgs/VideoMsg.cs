using NapCatScript.Core.JsonFormat;
using static NapCatScript.Core.JsonFormat.Msgs.VideoJson;

namespace NapCatScript.Core.JsonFormat.Msgs;

/// <summary>
/// 视频消息
/// </summary>
public class VideoMsg : BaseMsg
{
    public override string JsonText { get; set; }
    public override JsonElement JsonElement { get; set; }
    public override JsonDocument JsonDocument { get; set; }
    public override dynamic JsonObject { get; set; }

    /// <param name="file">本地路径或者网络路径, file://D:/a.mp4</param>
    public VideoMsg(string file)
    {
        VideoMsgData data = new VideoMsgData(file);
        VideoJson obj = new VideoJson(data);
        JsonText = JsonSerializer.Serialize(obj);
        JsonElement = JsonSerializer.SerializeToElement(obj);
        JsonDocument = JsonSerializer.SerializeToDocument(obj);
        JsonObject = obj;
    }
}

/// <summary>
/// 视频消息的Json对象
/// </summary>
public class VideoJson : MsgJson
{
    public VideoJson(VideoMsgData data)
    {
        Data = data;
        JsonText = JsonSerializer.Serialize(this);
    }

    [JsonPropertyName("type")]
    public MsgType Type { get; set; } = MsgType.video;//语音消息

    [JsonPropertyName("data")]
    public VideoMsgData Data { get; set; }
    [JsonIgnore]
    public override string JsonText { get; set; }

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

