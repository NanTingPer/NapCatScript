namespace NapCatScript.Core.JsonFormat.GroupManagers;

/// <summary>
/// 群踢人
/// </summary>
internal class set_group_kick : RequestJson
{
    /// <summary>
    /// 群踢人
    /// </summary>
    /// <param name="groupid">群id</param>
    /// <param name="userid">用户id</param>
    public set_group_kick(string groupid, string userid)
    {
        GroupId = groupid;
        UserId = userid;
        JsonText = JsonSerializer.Serialize(this);
    }

    /// <summary>
    /// 群踢人
    /// </summary>
    /// <param name="groupid">群id</param>
    /// <param name="userid">用户id</param>
    /// <param name="rejectAddRequest">是否拉黑</param>
    public set_group_kick(string groupid, string userid, bool rejectAddRequest) : this(groupid, userid)
    {
        RejectAddRequest = rejectAddRequest;
        JsonText = JsonSerializer.Serialize(this);
    }

    [JsonPropertyName("group_id")]
    public string GroupId { get; set; }

    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    [JsonPropertyName("reject_add_request")]
    public bool RejectAddRequest { get; set; } = false;
    public override string JsonText { get; set; }
}
