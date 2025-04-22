namespace NapCatScript.Core.JsonFormat.JsonModel;

/// <summary>
/// 删除好友
/// </summary>
/// <param name="user_id"> 用户id </param>
/// <param name="tempBlock"> 是否拉黑 </param>
/// <param name="tempBothDel"> 是否双向删除 </param>
public class delete_friend(string user_id, bool tempBlock, bool tempBothDel) : RequestJson
{
    public override string JsonText { get; set; } = JsonSerializer.Serialize(new Root(user_id, tempBlock, tempBothDel));
    public class Root(string user_id, bool tempBlock, bool tempBothDel)
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("friend_id")]
        public string? FriendId { get; set; }

        /// <summary>
        /// 拉黑
        /// </summary>
        [JsonPropertyName("temp_block")]
        public bool TempBlock { get; set; } = tempBlock;

        /// <summary>
        /// 双向删除
        /// </summary>
        [JsonPropertyName("temp_both_del")]
        public bool TempBothDel { get; set; } = tempBothDel;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("user_id")]
        public string? UserId { get; set; } = user_id;
    }

}
