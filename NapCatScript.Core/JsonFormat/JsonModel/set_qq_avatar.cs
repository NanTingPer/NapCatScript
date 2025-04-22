namespace NapCatScript.Core.JsonFormat.JsonModel;

/// <summary>
/// 设置QQ头像
/// </summary>
/// <param name="file">本地路径或网络路径"D:/a.jpg"</param>
public class set_qq_avatar(string file) : RequestJson
{
    public override string JsonText { get; set; } = JsonSerializer.Serialize(new Root(file));

    public class Root(string file)
    {
        /// <summary>
        /// 路径或链接
        /// </summary>
        [JsonPropertyName("file")]
        public string File { get; set; } = file;
    }
}
