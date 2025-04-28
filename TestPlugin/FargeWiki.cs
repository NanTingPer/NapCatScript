using NapCatScript.Core;
using TestPlugin.Models;

namespace TestPlugin;

public class FargeWiki : NapCatScript.Core.PluginType
{
    private static char s_splitString;
    private const string SPLITCONFNAME = "FargeWikiSplitChar";
    public override void Init()
    {
        string? str = GetConf(SPLITCONFNAME);
        str = (str is null | str == "") ? "?" : str;
        s_splitString = str!.Trim()[0];
    }

    public override async Task Run(MsgInfo mesg, string httpUri)
    {
        string nullTrim = mesg.MessageContent.Trim();
        if (!nullTrim.StartsWith(s_splitString))
            return;

        //starChar, content, #, commandContent
        //string allContent = nullTrim.Split(s_splitString)[1]; //取出content, #, commandContent
        //string[] contentAndCommand = allContent.Split("#"); //0是content 1是commandContent
        //string @bool = contentAndCommand[0];
        //string commandContent = contentAndCommand[1];

        //string command = nullTrim.Split('#')[1]; //是指令 也是ItemName
        //string commandValue = nullTrim.Split("#")[2];


        //0是空，1是全部内容
        string command = nullTrim.Split(s_splitString)[1]; //是指令 也是ItemName
        string @bool = command.Split("#")[0];
        switch (@bool) {
            case "映射":
                WikiNameMapping<FargeMapModel>.AddAsync(mesg, httpUri, command, CTokrn, [FargeNPCs, FargeItems]);
                return;
            case "删除映射":
                WikiNameMapping<FargeMapModel>.DeleteAsync(mesg, httpUri, command, CTokrn);
                return;
            case "查看全部映射":
                string mappings = await WikiNameMapping<FargeMapModel>.GetMappings();
                MsgJson json = new TextJson(mappings);
                Send.SendForawrd(mesg.GetId(), mesg, [json], mesg.GetMsgTo());
                return;
            case "更改触发字符": //startChar更改触发词#x
                char newStartChar = command.Split("#")[1].Trim()[0];
                if(newStartChar == '#') {
                    SendTextAsync(mesg, httpUri, "不能是#!", CTokrn);
                    return;
                }
                SetConf(SPLITCONFNAME, new StringBuilder().Append(newStartChar).ToString());
                s_splitString = newStartChar;
                SendTextAsync(mesg, httpUri, "Farge查询触发更改为: " + newStartChar, CTokrn);
                return;
        }

        string itemName = command;
        itemName = await WikiNameMapping<VanillaMapModel>.GetMap(itemName);
        string itemPath = Path.Combine(Environment.CurrentDirectory, "Farge" ,itemName + ".png");
        if (File.Exists(itemPath)) {
            ImageMesg image = new ImageMesg(mesg.GetId(), mesg.GetMsgTo(), itemPath);
            await SendMsg.Send(mesg.GetMsgToURL(httpUri), image.MesgString, null, CTokrn);
            return;
        }

        //未找到文件
        StringBuilder contains = new StringBuilder();
        contains.Append("猜你想找: ");
        AddString(contains, GetContains(FargeItems, itemName, 5), GetContains(FargeNPCs, itemName, 5));
        SendTextAsync(mesg, httpUri, contains.ToString(), CTokrn);
        await Task.Delay(0);
    }
}
