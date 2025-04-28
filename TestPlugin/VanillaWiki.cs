using NapCatScript.Core;
using TestPlugin.Models;

namespace TestPlugin;

public class VanillaWiki : NapCatScript.Core.PluginType
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
    }

    public override async Task Run(MsgInfo mesg, string httpUri)
    {
        string nullTrim = mesg.MessageContent.Trim();
        if (!nullTrim.StartsWith('*'))
            return;

        string command = nullTrim.Split('*')[1]; //是指令 也是ItemName
        string @bool = command.Split("#")[0];
        switch (@bool) {
            case "映射":
                WikiNameMapping<VanillaMapModel>.AddAsync(mesg, httpUri, command, CTokrn, [VNPCs, VItems]);
                return;
            case "删除映射":
                WikiNameMapping<VanillaMapModel>.DeleteAsync(mesg, httpUri, command, CTokrn);
                return;
            case "查看全部映射":
                string mappings = await WikiNameMapping<VanillaMapModel>.GetMappings();
                MsgJson json = new TextJson(mappings);
                Send.SendForawrd(mesg.GetId(), mesg, [json], mesg.GetMsgTo());
                return;
        }

        string itemName = command;
        itemName = await WikiNameMapping<VanillaMapModel>.GetMap(itemName);
        string itemPath = Path.Combine(Environment.CurrentDirectory, "Val" ,itemName + ".png");
        if (File.Exists(itemPath)) {
            ImageMesg image = new ImageMesg(mesg.GetId(), mesg.GetMsgTo(), itemPath);
            await SendMsg.Send(mesg.GetMsgToURL(httpUri), image.MesgString, null, CTokrn);
            return;
        }

        //未找到文件
        StringBuilder contains = new StringBuilder();
        contains.Append("猜你想找: ");
        AddString(contains, GetContains(VItems, itemName, 5), GetContains(VNPCs, itemName, 5));
        SendTextAsync(mesg, httpUri, contains.ToString(), CTokrn);
        await Task.Delay(0);
    }
}
