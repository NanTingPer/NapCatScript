using NapCatScript.Core;

namespace TestPlugin;

public class TestClass : PluginType
{
    public const string SKNAME = "DeepSeekRes";
    public static string DeepSeekKey { get; set; } = "";
    public static string StartString { get; set; } = "";
    public static Send? SendObj { get; private set; }
    public static string SkName { get; set; }

    static TestClass()
    {
        SkName = Config.GetConf(SKNAME) ?? "亭亭";
        SkName = SkName == "" ? "亭亭" : SkName;
    }

    public override void Init()
    {
        StartString = $"[CQ:at,qq={BotId}]";
        StartString = Regex.Replace(StartString, @"\s", "");
        DeepSeekKey = GetConf(Config.DeepSeekKey) ?? "";
        SendObj = Send;
    }

    public override async Task Run(MsgInfo mesg, string httpUri)
    {
        string mesgContent = mesg.MessageContent;
        mesgContent = Regex.Replace(mesgContent, @"\s", "");

        if(mesg.MessageContent.Contains("总结群消息")) {
            int a = 0;
        }

        var co = await FAQI.Get(mesgContent);
        if (co is not null) {
            string faq = co.Value.Replace("\\n", "\n");
            SendTextAsync(mesg, HttpUri, co.Value, CTokrn);//\r\n----来自:{co.UserName}
            //continue;
            return;
        }

        if (!mesgContent.StartsWith("亭亭$亭"))
            DeepSeekAPI.AddGroupMesg(mesg); //加入组

        if (string.IsNullOrEmpty(BotId)) {
            Log.Waring("配置文件中BotId未填, 无法使用DeepSeekAPI服务");
            return;
        }
        if (mesgContent.Contains(StartString) || mesgContent.Contains(SkName)) {
            try {
                DeepSeekAPI.SendAsync(mesg, httpUri, mesgContent, CTokrn);
            } catch (Exception E) {
                Console.WriteLine($"DeepSeek错误: {E.Message} \r\n {E.StackTrace}");
                Log.Erro(E.Message, E.StackTrace);
            }
            //continue;
            return;
        }
    }

    public static IEnumerable<string> GetContains(List<string> tarGetList, string containsString, int take)
    {
        return tarGetList.Where(f => f.Contains(containsString)).Take(take);
    }
}
