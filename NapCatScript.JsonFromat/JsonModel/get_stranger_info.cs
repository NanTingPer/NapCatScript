namespace NapCatScript.JsonFromat.JsonModel;

/// <summary>
/// 获取帐号信息
/// </summary>
public class get_stranger_info(string user_id) : RequestJson
{
    public override string JsonText { get; set; } = JsonSerializer.Serialize(new Root(user_id));

    private class Root(string user_id)
    {
        [JsonPropertyName("user_id")]
        public string User_id { get; set; } = user_id;
    }
}

/// <summary>
/// 获取帐号信息的返回值
/// </summary>
public class get_stranger_infoReturn
{
    /// <summary>
    /// 年龄
    /// </summary>
    public double Age { get => Data_.Age; }
    /// <summary>
    /// 是否为VIP
    /// </summary>
    public bool IsVip { get => Data_.IsVip; }
    /// <summary>
    /// 是否为年费VIP
    /// </summary>
    public bool IsYearsVip { get => Data_.IsYearsVip; }
    /// <summary>
    /// 登录天数
    /// </summary>
    public double LoginDays { get => Data_.LoginDays; }
    /// <summary>
    /// 个性签名
    /// </summary>
    public string LongNick { get => Data_.LongNick; }
    /// <summary>
    /// 昵称
    /// </summary>
    public string Nickname { get => Data_.Nickname; }
    public string Qid { get => Data_.Qid; }
    /// <summary>
    /// QQ等级
    /// </summary>
    public double QQLevel { get => Data_.QqLevel; }
    public double RegTime { get => Data_.RegTime; }
    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get => Data_.Remark; }
    /// <summary>
    /// 性别
    /// </summary>
    public string Sex { get => Data_.Sex; }
    public string Uid { get => Data_.Uid; }
    public string Uin { get => Data_.Uin; }
    public double UserId { get => Data_.UserId; }

    /// <summary>
    /// 会员等级
    /// </summary>
    [JsonPropertyName("vip_level")]
    public double VipLevel { get; set; }

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

    public partial class Data
    {
        /// <summary>
        /// 年龄
        /// </summary>
        [JsonPropertyName("age")]
        public double Age { get; set; }

        /// <summary>
        /// 是否会员
        /// </summary>
        [JsonPropertyName("is_vip")]
        public bool IsVip { get; set; }

        /// <summary>
        /// 是否年费会员
        /// </summary>
        [JsonPropertyName("is_years_vip")]
        public bool IsYearsVip { get; set; }

        /// <summary>
        /// 连续登录天数
        /// </summary>
        [JsonPropertyName("login_days")]
        public double LoginDays { get; set; }

        /// <summary>
        /// 个性签名
        /// </summary>
        [JsonPropertyName("long_nick")]
        public string LongNick { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [JsonPropertyName("nickname")]
        public string Nickname { get; set; }

        [JsonPropertyName("qid")]
        public string Qid { get; set; }

        /// <summary>
        /// 账号等级
        /// </summary>
        [JsonPropertyName("qqLevel")]
        public double QqLevel { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        [JsonPropertyName("reg_time")]
        public double RegTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [JsonPropertyName("remark")]
        public string Remark { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [JsonPropertyName("sex")]
        public string Sex { get; set; }

        [JsonPropertyName("status")]
        public double Status { get; set; }

        [JsonPropertyName("uid")]
        public string Uid { get; set; }

        [JsonPropertyName("uin")]
        public string Uin { get; set; }

        [JsonPropertyName("user_id")]
        public double UserId { get; set; }

        /// <summary>
        /// 会员等级
        /// </summary>
        [JsonPropertyName("vip_level")]
        public double VipLevel { get; set; }
    }

}
