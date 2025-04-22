namespace NapCatScript.Core.JsonFormat.JsonModel;

/// <summary>
/// 点赞
/// </summary>
/// <param name="user_id"> 目标用户id </param>
/// <param name="num"> 点击数量 </param>
public class send_like(string user_id, int num) : RequestJson
{
    public override string JsonText { get; set; } = JsonSerializer.Serialize(new Root(user_id, num));

    private class Root(string user_id, int num)
    {
        [JsonPropertyName("user_id")]
        public string User_id { get; set; } = user_id;

        [JsonPropertyName("times")]
        public int Times { get; set; } = num;
    }
}
