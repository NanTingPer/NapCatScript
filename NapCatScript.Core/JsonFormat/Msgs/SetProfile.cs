namespace NapCatScript.Core.JsonFormat.Msgs;
/// <summary>
/// 设置帐号信息
/// </summary>
public class SetProFile
{
    /// <summary>
    /// Json值
    /// </summary>
    public string JsonText { get; } = string.Empty;

    private SetProFile() { }

    /// <param name="nickname"> 昵称 </param>
    /// <param name="personalNote"> 个性签名 </param>
    /// <param name="sex"> 性别 </param>
    public SetProFile(string nickname, string personalNote, string sex)
    {
        Root rootJson = new Root(nickname, personalNote, sex);
        JsonText = JsonSerializer.SerializeToDocument(rootJson).RootElement.GetString()!;
    }

    public class Root
    {
        public Root(string nickname, string personalNote, string sex)
        {
            Nickname = nickname;
            PersonalNote = personalNote;
            Sex = sex;
        }

        /// <summary>
        /// 昵称
        /// </summary>
        [JsonPropertyName("nickname")]
        public string Nickname { get; set; } = string.Empty;

        /// <summary>
        /// 个性签名
        /// </summary>
        [JsonPropertyName("personal_note")]
        public string PersonalNote { get; set; } = string.Empty;

        /// <summary>
        /// 性别
        /// </summary>
        [JsonPropertyName("sex")]
        public string Sex { get; set; } = string.Empty;

    }
}

