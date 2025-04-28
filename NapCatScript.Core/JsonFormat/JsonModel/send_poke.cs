namespace NapCatScript.Core.JsonFormat.JsonModel;

/// <summary>
/// 戳一戳
/// </summary>
internal class send_poke : RequestJson
{
    /// <summary>
    /// 群戳一戳
    /// </summary>
    /// <param name="groupid">群id</param>
    /// <param name="userid">用户id</param>
    public send_poke(string groupid, string userid)
    {
        GroupId = groupid;
        UserId = userid;
        JsonText = JsonSerializer.Serialize(this);
    }

    /// <summary>
    /// 私聊戳一戳
    /// </summary>
    /// <param name="userid">用户id</param>
    public send_poke(string userid)
    {
        UserId = userid;
        JsonText = JsonSerializer.Serialize(this);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("group_id")]
    public string? GroupId { get; set; }

    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    public override string JsonText { get; set; }
}
