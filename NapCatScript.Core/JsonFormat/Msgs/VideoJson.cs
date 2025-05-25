namespace NapCatScript.Core.JsonFormat.Msgs;

/// <summary>
/// 视频消息的Json对象
/// </summary>
public class VideoJson : MsgJson
{
    #region ctor
    
    [JsonConstructor]
    private VideoJson()
    {
        
    }
    
    public VideoJson(VideoJsonData data)
    {
        Data = data;
    }

    /// <param name="filePath">本地路径或者网络路径, file://D:/a.mp4</param>
    public VideoJson(string filePath) : this(new VideoJsonData(filePath)) { }

    #endregion
    
    [JsonPropertyName("type")]
    public MsgType Type { get; set; } = MsgType.video;//语音消息

    [JsonPropertyName("data")]
    public VideoJsonData Data { get; set; }

    //JsonClass

    public class VideoJsonData
    {
        [JsonConstructor]
        private VideoJsonData() { }
        
        public VideoJsonData(string content)
        {
            file = content;
        }

        [JsonPropertyName("file")]
        public string file { get; set; }
    }


}

