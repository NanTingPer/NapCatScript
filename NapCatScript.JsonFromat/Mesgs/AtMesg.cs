﻿namespace NapCatScript.JsonFromat.Mesgs;

public class AtMesg : BaseMesg
{
    public override string JsonText { get; set; }
    public AtMesg(string qqid)
    {
        JsonText = JsonSerializer.Serialize(new Root(qqid));
    }

    public AtMesg(string name, string qqid)
    {
        JsonText = JsonSerializer.Serialize(new Root(name, qqid));
    }

    public partial class Root
    {
        public Root(string qqid)
        {
            Message = new Data(qqid);
        }

        public Root(string name, string qqid)
        {
            Message = new Data(name, qqid);
        }

        [JsonPropertyName("message")]
        public Data Message { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = MsgType.at.ToString();
    }

    public class Data
    {
        public Data(string qqid)
        {
            QQ = qqid;
        }

        public Data(string name, string qqid)
        {
            Name = name;
            QQ = qqid;
        }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("qq")]
        public string QQ { get; set; }
    }
}