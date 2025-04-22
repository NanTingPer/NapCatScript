namespace NapCatScript.Core.JsonFormat.JsonModel;

/// <summary>
/// 设置个性签名
/// </summary>
/// <param name="content">内容</param>
public class set_self_longnick(string content) : RequestJson
{
    public override string JsonText { get; set; } = JsonSerializer.Serialize(new Root(content));

    private class Root(string content)
    {
        [JsonPropertyName("longNick")]
        public string LongNick { get; set; } = content;
    }
}
