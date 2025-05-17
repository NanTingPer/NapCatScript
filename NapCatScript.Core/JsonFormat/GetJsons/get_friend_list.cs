namespace NapCatScript.Core.JsonFormat.JsonModel;

/// <summary>
/// 获取好友列表
/// </summary>
public class get_friend_list(bool no = false) : RequestJson
{
    public override string JsonText { get; set; } = JsonSerializer.Serialize(new Root(no));

    private class Root(bool no = false)
    {
        [JsonPropertyName("no_cache")]
        public bool NoCache { get; set; } = no;
    }
}

public class get_friend_listReturn
{
    /// <summary>
    /// 值
    /// </summary>
    [JsonPropertyName("data")]
    public List<Datum> Values { get; set; }

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

    public class Datum
    {
        /// <summary>
        /// 年龄
        /// </summary>
        [JsonPropertyName("age")]
        public double Age { get; set; }

        /// <summary>
        /// 生日_日
        /// </summary>
        [JsonPropertyName("birthday_day")]
        public double BirthdayDay { get; set; }

        /// <summary>
        /// 生日_月
        /// </summary>
        [JsonPropertyName("birthday_month")]
        public double BirthdayMonth { get; set; }

        /// <summary>
        /// 生日_年
        /// </summary>
        [JsonPropertyName("birthday_year")]
        public double BirthdayYear { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        [JsonPropertyName("categoryId")]
        public double CategoryId { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [JsonPropertyName("eMail")]
        public string EMail { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        [JsonPropertyName("level")]
        public double Level { get; set; }

        /// <summary>
        /// 个性签名
        /// </summary>
        [JsonPropertyName("longNick")]
        public string LongNick { get; set; }

        [JsonPropertyName("nick")]
        public string Nick { get; set; }

        [JsonPropertyName("nickname")]
        public string Nickname { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [JsonPropertyName("phoneNum")]
        public string PhoneNum { get; set; }

        /// <summary>
        /// QQID
        /// </summary>
        [JsonPropertyName("qid")]
        public string Qid { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [JsonPropertyName("remark")]
        public string Remark { get; set; }

        [JsonPropertyName("richBuffer")]
        public Dictionary<string, object> RichBuffer { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        [JsonPropertyName("richTime")]
        public double RichTime { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [JsonPropertyName("sex")]
        public string Sex { get; set; }

        [JsonPropertyName("uid")]
        public string Uid { get; set; }

        [JsonPropertyName("uin")]
        public string Uin { get; set; }

        [JsonPropertyName("user_id")]
        public double UserId { get; set; }
    }

}

