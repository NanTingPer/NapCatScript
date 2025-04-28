namespace NapCatScript.Core.JsonFormat.GroupManagers;

/// <summary>
/// 设置群备注
/// </summary>
/// <param name="groupId"></param>
/// <param name="name"></param>
internal class set_group_remark : RequestJson
{
    public override string JsonText { get; set; }
    public set_group_remark(string groupId, string name)
    {
        GroupId = groupId;
        Name = name;
        JsonText = JsonSerializer.Serialize(this);
    }

    [JsonPropertyName("group_id")]
    public string GroupId { get; set; }

    [JsonPropertyName("remark")]
    public string Name { get; set; }
}
