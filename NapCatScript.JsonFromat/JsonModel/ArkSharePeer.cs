namespace NapCatScript.JsonFromat.JsonModel;

/// <summary>
/// 获取推荐好友/群聊卡片
/// </summary>
public class ArkSharePeer(string id, ArkSharePeerEnum type) : BaseMesg //获取推荐好友/群聊卡片
{
    public override string JsonText { get; set; } = JsonSerializer.Serialize(new Root(id, type));

    private class Root
    {
        public Root(string id, ArkSharePeerEnum type)
        {
            if(type == ArkSharePeerEnum.User_id) {
                User_id = id;
            } else {
                Group_id = id;
            }
        }


        [JsonPropertyName("group_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Group_id { get; set; } = null;

        [JsonPropertyName("user_id")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? User_id { get; set; } = null;

        /// <summary>
        /// 对方手机号
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("phoneNumber")]
        public string? PhoneNumber { get; set; }
    }
}

public enum ArkSharePeerEnum
{
    User_id,
    Group_id
}
