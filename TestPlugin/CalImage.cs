using NapCatScript.JsonFromat.Mesgs;
using NapCatScript.MesgHandle.Parses;

namespace TestPlugin;

public class CalImage : PluginType
{
    public override void Init()
    {
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
                CalMapping.AddAsync(mesg, HttpUri, txtContent, CTokrn);
                //continue;
                return;
            } else if (txtContent.StartsWith("删除映射#")) {
                CalMapping.DeleteAsync(mesg, HttpUri, txtContent, CTokrn);
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
            txtContent = await CalMapping.GetMap(txtContent);
            string filePath = Path.Combine(Environment.CurrentDirectory, "Cal", txtContent + ".png");
            string sendUrl = HUtils.GetMsgURL(HttpUri, mesg, out MesgTo MESGTO);
            CalImage.SendAsync(mesg, txtContent, filePath, sendUrl, MESGTO);
            return;
        }
    }

    public static async void/*Task<string>*/ SendAsync(MesgInfo mesg, string fileName, string filePath, string sendUrl, MesgTo MESGTO)
    {
        if (string.IsNullOrEmpty(fileName))
            fileName = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
        if (File.Exists(filePath)) {
            ImageMesg sendMesg = new ImageMesg(GetUserId(mesg), MESGTO, filePath);
            await SendMesg.Send(sendUrl, sendMesg.MesgString, null, CTokrn);
            return;
        }
        Console.WriteLine("未找到文件: " + filePath);
        #region 猜你想找
        StringBuilder 猜你想找 = new StringBuilder();
        猜你想找.Append("猜你想找: ");
        //skip跳
        //take取
        IEnumerable<string> contentItem = ContentList.ItemName.Where(str => str.Contains(fileName)).Take(5);
        IEnumerable<string> contentNpc = ContentList.NPCName.Where(str => str.Contains(fileName)).Take(5);
        List<string> 随机 = [];
        int take = contentItem.Count() + contentNpc.Count();
        if (take < 10) {
            for (int i = take; i < 10; i++) {
                int randInt = rand.Next(0, 2);
                if (randInt == 1)
                    随机.Add(ContentList.ItemName[rand.Next(0, ContentList.ItemName.Count)]);
                if (randInt == 0)
                    随机.Add(ContentList.NPCName[rand.Next(0, ContentList.NPCName.Count)]);
            }
        }
        AddString(猜你想找, contentNpc, contentItem, 随机.AsEnumerable());

        TextMesg tmesg = new TextMesg(GetUserId(mesg), MESGTO, 猜你想找.ToString().Substring(0, 猜你想找.Length - 2));
        await SendMesg.Send(sendUrl, tmesg.MesgString, null, CTokrn);
        return;
        #endregion
    }
}
