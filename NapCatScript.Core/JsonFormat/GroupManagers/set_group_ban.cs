namespace NapCatScript.Core.JsonFormat.GroupManagers;

/// <summary>
/// 群禁言
/// </summary>
internal class set_group_ban : RequestJson
{
    /// <summary>
    /// 群禁言
    /// </summary>
    /// <param name="group_id">群id</param>
    /// <param name="user_id">用户id</param>
    /// <param name="duration">禁言时长</param>
    public set_group_ban(string group_id, string user_id, double duration)
    {
        GroupId = group_id;
        UserId = user_id;
        Duration = duration;
        JsonText = JsonSerializer.Serialize(this);
    }

    [JsonPropertyName("group_id")]
    public string GroupId { get; set; }

    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    [JsonPropertyName("duration")]
    public double Duration { get; set; }
    public override string JsonText { get; set; }
}
