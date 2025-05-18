namespace NapCatScript.Core.JsonFormat.GetJsons;

/// <summary>
/// 获取群列表
/// </summary>
public class get_group_list
{
    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("retcode")]
    public int Retcode { get; set; }

    [JsonPropertyName("data")]
    public List<GroupInfo> GroupInfos { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("wording")]
    public string Wording { get; set; }

    [JsonPropertyName("echo")]
    public string Echo { get; set; }
}

/// <summary>
///       "group_all_shut": 0,
/// <para> "group_remark": "", </para>        
/// <para> "group_id": , </para>        
/// <para> "group_name": "", </para>        
/// <para> "member_count": 3, </para>        
/// <para> "max_member_count": 200 </para>        
/// </summary>
public class GroupInfo
{
    [JsonPropertyName("max_member_count")]
    public int MaxMemberCount { get; set; }
    
    [JsonPropertyName("member_count")]
    public int MemberCount { get; set; }
    
    /// <summary>
    /// 群名称
    /// </summary>
    [JsonPropertyName("group_name")]
    public string GroupName { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    [JsonPropertyName("group_all_shut")]
    public int GroupAllShut { get; set; }

    /// <summary>
    /// 群备注
    /// </summary>
    [JsonPropertyName("group_remark")]
    public string GroupRemark { get; set; }

    /// <summary>
    /// 群号
    /// </summary>
    [JsonPropertyName("group_id")]
    public long GroupId { get; set; }
}