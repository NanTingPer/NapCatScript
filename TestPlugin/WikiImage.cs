using NapCatScript.JsonFromat.Mesgs;
using NapCatScript.MesgHandle.Parses;
using System.Threading.Tasks;
using static TestPlugin.ContentList;

namespace TestPlugin;

public class WikiImage : PluginType
{
    public override void Init()
    {
        VNPCs = VNPCs.Distinct().ToList();

        VItems = VItems.Distinct().Select(f => {
            if (f.Contains("火把")) return "火把";
            if (f.Contains("旗") && !f.Contains("哥布林") && f.Length > 2) return "敌怪旗";
            if (f.Contains("链甲")) return f.Replace("链甲", "盔甲");
            if (f.Contains("护胫")) return f.Replace("护胫", "盔甲");
            if (f.Contains("兜帽")) return f.Replace("兜帽", "盔甲");
            if (f.Contains("八音盒")) return "八音盒";
            if (f.Contains("椅")) return "椅子";
            if (f.Contains("桌")) return "桌子";
            if (f.Contains("灯笼")) return "灯笼";
            if (f.Contains("火把")) return "火把";
            if (f.Contains("雕像")) return "雕像";
            if (f.Contains("钩")) return "钩爪";
            if (f.Contains("盆栽")) return "盆栽";
            if (f.Contains("矿车")) return "矿车";
            if (f.Contains("纪念章")) return "纪念章";
            if (f.Contains("床")) return "床";
            if (f.Contains("书架")) return "书架";
            if (f.Contains("工作台") && !f.Contains("重型")) return "工作台";
            if (f.Contains("音乐盒")) return "八音盒";
            return f.Replace("/", "-");
        }).Distinct().ToList();
        Console.WriteLine("加载CalImage!");
    }

    public override async Task Run(MesgInfo mesg, string httpUri)
    {
        await Task.Delay(0);
        string mesgContent = mesg.MessageContent;
        mesgContent = Regex.Replace(mesgContent, @"\s", "");
        if (mesgContent.Trim().StartsWith('.')) {
            //string[] mesgs = mesgContent.Split(".");
            string txtContent;//消息内容 = mesgs[1]
            txtContent = mesgContent.Trim().Substring(1);
            if (txtContent.StartsWith("映射#")) {
                WikiNameMapping.AddAsync(mesg, HttpUri, txtContent, CTokrn);
                //continue;
                return;
            } else if (txtContent.StartsWith("查看全部映射#")) {
                string mappings = await WikiNameMapping.GetMappings();
                TextMsgJson json = new TextMsgJson(mappings);
                Send.SendForawrd(mesg.GetId(), mesg, [json], mesg.GetMesgTo());
                return;
            } else if (txtContent.StartsWith("删除映射#")) {
                WikiNameMapping.DeleteAsync(mesg, HttpUri, txtContent, CTokrn);
                return;//continue;
            } else if (txtContent.StartsWith("FAQ#")) {
                FAQI.AddAsync(mesg, HttpUri, txtContent, CTokrn);
                return;//continue;
            } else if (txtContent.StartsWith("删除FAQ#")) {
                FAQI.DeleteAsync(txtContent);
                SendTextAsync(mesg, HttpUri, "好啦好啦，删掉啦", CTokrn);
                return;//continue;
            } else if (txtContent.StartsWith("help#")) {
                SendTextAsync(mesg, HttpUri,
                    """
                            对于灾厄Wiki: 
                                1. 使用"." + 物品名称 可以获得对应物品的wiki页, 例 .震波炸弹
                                2. 使用".映射#" 可以设置对应物品映射, 例   .映射#神明吞噬者=>神吞
                                3. 使用".删除映射#" 可以删除对应映射, 例   .删除映射#神吞
                            对于FAQ:
                                1. 使用".FAQ#" 可以创建FAQ     例      .FAQ#灾厄是什么###灾厄是一个模组
                                2. 使用".删除FAQ#" 可以删除FAQ 例      .删除FAQ#灾厄是什么

                            """, CTokrn);
                // continue;
                return;
            }
            txtContent = await WikiNameMapping.GetMap(txtContent);
            string calFilePath = Path.Combine(Environment.CurrentDirectory, "Cal", txtContent + ".png");
//#if DEBUG
//            string valFilePath = Path.Combine(@"D:\OpenSource\Create\灾厄WIKI\Val", txtContent + ".png");
//#else
            string valFilePath = Path.Combine(Environment.CurrentDirectory, "Val", txtContent + ".png");
//#endif
            string sendUrl = GetMsgURL(HttpUri, mesg, out MesgTo MESGTO);
            SendAsync(mesg, txtContent,  sendUrl, MESGTO, [calFilePath, valFilePath]);
            return;
        }
    }

    public static async void/*Task<string>*/ SendAsync(MesgInfo mesg, string fileName, string sendUrl, MesgTo MESGTO, params string[] filePaths)
    {
        if (string.IsNullOrEmpty(fileName))
            fileName = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        bool sendAvtice = false;
        foreach (var file in filePaths) {
            sendAvtice = await SendImage(mesg, file, sendUrl, MESGTO) || sendAvtice;
        }
        if (sendAvtice) {
            return;
        }

        Loging.Log.Info($"未找到文件: {filePaths}");
        #region 猜你想找
        StringBuilder 猜你想找 = new StringBuilder();
        猜你想找.Append("猜你想找 \n");
        猜你想找.Append("灾厄: ");

        //skip跳
        //take取
        IEnumerable<string> contentItem = GetContains(ItemName, fileName, 3);
        IEnumerable<string> contentNpc = GetContains(NPCName, fileName, 3);
        List<string> 随机 = [];
        //int take = contentItem.Count() + contentNpc.Count();
        //if (take < 10) {
        //    for (int i = take; i < 10; i++) {
        //        int randInt = rand.Next(0, 2);
        //        if (randInt == 1)
        //            随机.Add(ContentList.ItemName[rand.Next(0, ContentList.ItemName.Count)]);
        //        if (randInt == 0)
        //            随机.Add(ContentList.NPCName[rand.Next(0, ContentList.NPCName.Count)]);
        //    }
        //}
        AddString(猜你想找, contentNpc, contentItem/*, 随机.AsEnumerable()*/);
        猜你想找.Append("\n原版: ");
        IEnumerable<string> vcontentItem = GetContains(VItems, fileName, 3);
        IEnumerable<string> vcontentNpc = GetContains(VNPCs, fileName, 3);
        AddString(猜你想找, vcontentItem, vcontentNpc);
        TextMesg tmesg = new TextMesg(GetUserId(mesg), MESGTO, 猜你想找.ToString().Substring(0, 猜你想找.Length - 2));
        await SendMesg.Send(sendUrl, tmesg.MesgString, null, CTokrn);
        return;
        #endregion
    }

    private static async Task<bool> SendImage(MesgInfo mesg, string filePath, string sendUrl, MesgTo MESGTO)
    {
        if (File.Exists(filePath)) {
            ImageMesg sendMesg = new ImageMesg(GetUserId(mesg), MESGTO, filePath);
            await SendMesg.Send(sendUrl, sendMesg.MesgString, null, CTokrn);
            return true;
        }
        return false;
    }

    public static IEnumerable<string> GetContains(List<string> tarGetList, string containsString, int take)
    {
        return tarGetList.Where(f => f.Contains(containsString)).Take(take);
    }
}
