using NapCatScript.JsonFromat.Mesgs;
using System.Text;

namespace NapCatScript.JsonFromat;

/// <summary>
/// 最终消息类
/// </summary>
public class MesgHandle : BaseMesg
{
    public override string JsonText { get; set; }
    /// <summary>
    /// 构建 使用MessagesJsonObject获取
    /// </summary>
    /// <param name="user_id"> 用户id </param>
    /// <param name="text"> 要发送的消息 </param>
    public MesgHandle(string user_id, MesgTo mestype, params BaseMesg[] mesgs)
    {
        List<string> contents = [];
        foreach (var item in mesgs) {
            contents.Add(item.ToString());
        }

        Root root = new Root(user_id, contents);
        JsonText = JsonSerializer.Serialize(root);
        if (mestype == MesgTo.group) {
            string[] strings = JsonText.Split("user_id");
            StringBuilder sbuilder = new StringBuilder();
            sbuilder.Append(strings[0]);
            sbuilder.Append("group_id");
            for (int i = 1; i < strings.Length; i++) {
                sbuilder.Append(strings[i]);
            }
            JsonText = sbuilder.ToString();
        }
    }


    public class Root
    {
        public Root(string user_id, List<string> message)
        {
            User_id = user_id;
            Message = message;
        }

        [JsonPropertyName("user_id")]
        public string User_id { get; set; }

        [JsonPropertyName("message")]
        public List<string> Message { get; set; }
    }
}
