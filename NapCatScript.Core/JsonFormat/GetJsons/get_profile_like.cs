namespace NapCatScript.Core.JsonFormat.JsonModel;

/// <summary>
/// 获取点赞列表，请求无需json {}
/// </summary>
public class get_profile_like
{
    /// <summary>
    /// 用户信息列表
    /// </summary>
    public List<UserInfo> UserInfos { get => Data_.UserInfos; }

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
        [JsonPropertyName("last_visit_time")]
        public double LastVisitTime { get; set; }

        /// <summary>
        /// 新点赞数
        /// </summary>
        [JsonPropertyName("new_count")]
        public double NewCount { get; set; }

        [JsonPropertyName("new_nearby_count")]
        public double NewNearbyCount { get; set; }

        /// <summary>
        /// 总点赞数
        /// </summary>
        [JsonPropertyName("total_count")]
        public double TotalCount { get; set; }

        [JsonPropertyName("userInfos")]
        public List<UserInfo> UserInfos { get; set; }
    }

    public class UserInfo
    {
        [JsonPropertyName("age")]
        public double Age { get; set; }

        [JsonPropertyName("bAvailableCnt")]
        public double BAvailableCnt { get; set; }

        [JsonPropertyName("bTodayVotedCnt")]
        public double BTodayVotedCnt { get; set; }

        [JsonPropertyName("count")]
        public double Count { get; set; }

        [JsonPropertyName("customId")]
        public double CustomId { get; set; }

        [JsonPropertyName("gender")]
        public double Gender { get; set; }

        [JsonPropertyName("giftCount")]
        public double GiftCount { get; set; }

        [JsonPropertyName("isFriend")]
        public bool IsFriend { get; set; }

        [JsonPropertyName("isSvip")]
        public bool IsSvip { get; set; }

        [JsonPropertyName("isvip")]
        public bool Isvip { get; set; }

        [JsonPropertyName("lastCharged")]
        public double LastCharged { get; set; }

        [JsonPropertyName("latestTime")]
        public double LatestTime { get; set; }

        [JsonPropertyName("nick")]
        public string Nick { get; set; }

        [JsonPropertyName("src")]
        public double Src { get; set; }

        [JsonPropertyName("uid")]
        public string Uid { get; set; }

        [JsonPropertyName("uin")]
        public double Uin { get; set; }
    }

}
