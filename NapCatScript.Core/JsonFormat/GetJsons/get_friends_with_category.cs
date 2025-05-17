namespace NapCatScript.Core.JsonFormat.JsonModel;

/// <summary>
/// 获取好友信息分组列表（返回）
/// </summary>
public class get_friends_with_category
{
    [JsonPropertyName("data")]
    public List<Datum> Data { get; set; }

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
        /// 好友列表
        /// </summary>
        [JsonPropertyName("buddyList")]
        public List<好友信息> BuddyList { get; set; }

        /// <summary>
        /// 分组ID
        /// </summary>
        [JsonPropertyName("categoryId")]
        public double CategoryId { get; set; }

        /// <summary>
        /// 好友数量
        /// </summary>
        [JsonPropertyName("categoryMbCount")]
        public double CategoryMbCount { get; set; }

        /// <summary>
        /// 分组名
        /// </summary>
        [JsonPropertyName("categoryName")]
        public string CategoryName { get; set; }

        /// <summary>
        /// 分组排序ID
        /// </summary>
        [JsonPropertyName("categorySortId")]
        public double CategorySortId { get; set; }

        /// <summary>
        /// 在线好友数量
        /// </summary>
        [JsonPropertyName("onlineCount")]
        public double OnlineCount { get; set; }
    }

    /// <summary>
    /// 好友信息
    /// </summary>
    public class 好友信息
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
