namespace NapCatScript.Core.JsonFormat.JsonModel;

/// <summary>
/// 上传私聊文件
/// </summary>
/// <param name="user_id"> 用户id </param>
/// <param name="file"> 文件路径 </param>
/// <param name="name"> 目标名称 </param>
public class upload_private_file(string user_id, string file, string name) : RequestJson
{
    public override string JsonText { get; set; } = JsonSerializer.Serialize(new Root(user_id, file, name));
    public class Root(string user_id, string file, string name)
    {
        [JsonPropertyName("file")]
        public string File { get; set; } = file;

        [JsonPropertyName("name")]
        public string Name { get; set; } = name;

        [JsonPropertyName("user_id")]
        public string UserId { get; set; } = user_id;
    }

}
