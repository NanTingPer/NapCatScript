using NapCatScript.JsonFromat;
using NapCatScript.Services;
using static NapCatScript.Start.Main_;
using static NapCatScript.Start.FAQ;

using static NapCatScript.MesgHandle.Utils;
using HUtils = NapCatScript.MesgHandle.Utils;

namespace TestPlugin;

public class TestClass : PluginType
{
    public static string DeepSeekKey { get; set; } = "";
    public static string StartString { get; set; } = "";
    public override void Init()
    {
        StartString = $"[CQ:at,qq={BotId}]";
        StartString = Regex.Replace(StartString, @"\s", "");
        DeepSeekKey = GetConf(Config.DeepSeekKey) ?? "";
        Console.WriteLine("Hello Is My Plugin!");
    }

    public override async Task Run(MesgInfo mesg, string httpUri)
    {
        string mesgContent = mesg.MessageContent;
        mesgContent = Regex.Replace(mesgContent, @"\s", "");
        if (!mesgContent.StartsWith("亭亭$亭"))
            DeepSeekAPI.AddGroupMesg(mesg); //加入组
        if (mesgContent.StartsWith(StartString) || mesgContent.StartsWith("亭亭")) {
            try {
                DeepSeekAPI.SendAsync(mesg, httpUri, mesgContent, CTokrn);
            } catch (Exception E) {
                Console.WriteLine($"DeepSeek错误: {E.Message} \r\n {E.StackTrace}");
                Log.Erro(E.Message, E.StackTrace);
            }
            //continue;
            return;
        }

        var co = await FAQI.Get(mesgContent);
        if (co is not null) {
            SendTextAsync(mesg, HttpUri, $"{co.Value}\r\n----来自:{co.UserName}", CTokrn);
            //continue;
            return;
        }

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
                return;// continue;
            }

            txtContent = await CalMapping.GetMap(txtContent);
            string filePath = Path.Combine(Environment.CurrentDirectory, "Cal", txtContent + ".png");
            string sendUrl = HUtils.GetMsgURL(HttpUri, mesg, out MesgTo MESGTO);
            CalImage.SendAsync(mesg, txtContent, filePath, sendUrl, MESGTO);
        }
    }
}
