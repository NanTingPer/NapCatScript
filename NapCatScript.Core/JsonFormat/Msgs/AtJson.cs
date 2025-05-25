using System.Diagnostics.CodeAnalysis;

namespace NapCatScript.Core.JsonFormat.Msgs;

/// <summary>
/// at消息的json
/// </summary>
public class AtJson : MsgJson
{
    #region ctor

    [JsonConstructor]
    private AtJson()
    {
        
    }
    
    public AtJson(string qqid)
    {
        Data = new AtMsgData(qqid);
    }

    public AtJson(string name, string qqid)
    {
        Data = new AtMsgData(name, qqid);
    }

    #endregion
    
    [JsonPropertyName("data")]
    public AtMsgData Data { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; } = nameof(MsgType.at);

    public class AtMsgData
    {
        [JsonConstructor]
        private AtMsgData()
        {
            
        }
        
        [SetsRequiredMembers]
        public AtMsgData(string qqid)
        {
            QQ = qqid;
        }
        
        [SetsRequiredMembers]
        public AtMsgData(string name, string qqid)
        {
            Name = name;
            QQ = qqid;
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("name")]
        public string? Name { get; set; } = null;

        [JsonPropertyName("qq")]
        public required string QQ { get; set; }
    }
}


