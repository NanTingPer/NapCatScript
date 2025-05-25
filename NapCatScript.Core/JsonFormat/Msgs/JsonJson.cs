using System.Diagnostics.CodeAnalysis;

namespace NapCatScript.Core.JsonFormat.Msgs;

/// <summary>
/// Json消息的Json对象
/// </summary>
public class JsonJson : MsgJson
{
    #region ctor

    [JsonConstructor]
    private JsonJson()
    {
        
    }
    
    public JsonJson(JsonMsgData data)
    {
        Data = data;
    }

    #endregion

    [JsonPropertyName("type")]
    public MsgType Type { get; set; } = MsgType.json;

    [JsonPropertyName("data")]
    public JsonMsgData Data { get; set; }

    //JsonClass
    public class JsonMsgData
    {
        [JsonConstructor]
        private JsonMsgData()
        {
            
        }
        
        public JsonMsgData(string content)
        {
            data = content;
        }

        [JsonPropertyName("data")]
        public string data { get; set; }
    }
}


