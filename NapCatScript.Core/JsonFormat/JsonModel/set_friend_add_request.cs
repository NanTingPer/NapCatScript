namespace NapCatScript.Core.JsonFormat.JsonModel;

/// <summary>
/// 处理好友请求
/// </summary>
/// <param name="flag"> 目标id </param>
/// <param name="approve"> 是否同意 </param>
/// <param name="remark"> 设置备注 </param>
public class set_friend_add_request(string flag, bool approve, string remark) : RequestJson
{
    public override string JsonText { get; set; } = JsonSerializer.Serialize(new Root(flag, approve, remark));

    private class Root(string flag, bool approve, string remark)
    {
        /// <summary>
        /// 是否同意
        /// </summary>
        [JsonPropertyName("approve")]
        public bool Approve { get; set; } = approve;

        /// <summary>
        /// 请求id
        /// </summary>
        [JsonPropertyName("flag")]
        public string Flag { get; set; } = flag;

        /// <summary>
        /// 好友备注
        /// </summary>
        [JsonPropertyName("remark")]
        public string Remark { get; set; } = remark;
    }
}
