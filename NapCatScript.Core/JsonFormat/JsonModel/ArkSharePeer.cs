namespace NapCatScript.Core.JsonFormat.JsonModel;

/// <summary>
/// 获取推荐好友/群聊卡片
/// </summary>
public class ArkSharePeer(string id, ArkSharePeerEnum type) : RequestJson //获取推荐好友/群聊卡片
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

/// <summary>
/// ArkSharePeer的返回Json
/// </summary>
public class ArkSharePeerReturn
{
    [JsonIgnore]
    public string ArkJson { get => Data_.ArkJson; }

    [JsonPropertyName("data")]
    public Data Data_ { get; set; }

    [JsonPropertyName("echo")]
    public string Echo { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("retcode")]
    public double Retcode { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("wording")]
    public string Wording { get; set; }
    public class Data
    {
        /// <summary>
        /// 卡片json
        /// </summary>
        [JsonPropertyName("arkJson")]
        public string ArkJson { get; set; }

        [JsonPropertyName("errCode")]
        public double ErrCode { get; set; }

        [JsonPropertyName("errMsg")]
        public string ErrMsg { get; set; }
    }
}


