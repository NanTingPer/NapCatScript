namespace NapCatScript.Core.JsonFormat.Msgs;

public class RecordJson : MsgJson
{
    #region ctor
    
    [JsonConstructor]
    private RecordJson()
    {
        
    }
    
    public RecordJson(RecordMsgData data)
    {
        Data = data;
    }

    #endregion
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="filePath">本地路径或者网络路径, file://D:/a.mp3</param>
    public RecordJson(string filePath)
    {
        Data = new RecordMsgData(filePath);
    }

    [JsonPropertyName("type")]
    public MsgType Type { get; set; } = MsgType.record;//语音消息

    [JsonPropertyName("data")]
    public RecordMsgData Data { get; set; }

    public class RecordMsgData
    {
        [JsonConstructor]
        private RecordMsgData()
        {
            
        }
        
        public RecordMsgData(string content)
        {
            file = content;
        }

        [JsonPropertyName("file")]
        public string file { get; set; }
    }

    //JsonClass
}


