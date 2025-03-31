using System;

namespace NapCatScript.JsonFromat.Mesgs;

/// <summary>
/// Json卡片消息
/// </summary>
public class JsonMesg : BaseMesg
{
    private string JsonText { get; set; } = string.Empty;
    
    public JsonMesg(string content)
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
        public MsgType Type { get; set; } = MsgType.json;

        [JsonPropertyName("data")]
        public Data Data { get; set; }

        //JsonClass
    }

    public class Data
    {
        public Data(string content)
        {
            data = content;
        }

        [JsonPropertyName("data")]
        public string data { get; set; }
    }

    public override string GetString()
    {
        return JsonText;
    }
}
