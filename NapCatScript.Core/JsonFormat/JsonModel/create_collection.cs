namespace NapCatScript.Core.JsonFormat.JsonModel;

/// <summary>
/// 创建收藏内容
/// </summary>
/// <param name="bried"> 收藏标题 </param>
/// <param name="rowdata"> 收藏内容 </param>
public class create_collection(string bried, string rowdata) : RequestJson
{
    public override string JsonText { get; set; } = JsonSerializer.Serialize(new Root(bried, rowdata));

    private class Root(string brief, string rowdata)
    {
        /// <summary>
        /// 标题
        /// </summary>
        [JsonPropertyName("brief")]
        public string Brief { get; set; } = brief;

        /// <summary>
        /// 内容
        /// </summary>
        [JsonPropertyName("rawData")]
        public string RawData { get; set; } = rowdata;
    }
}
