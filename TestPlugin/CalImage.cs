using NapCatScript.JsonFromat;
using NapCatScript.JsonFromat.Mesgs;
using NapCatScript.MesgHandle.Parses;
using static NapCatScript.Start.Main_;
using static NapCatScript.MesgHandle.Utils;
using NapCatScript.Model;
using System.Text;

namespace TestPlugin;

public static class CalImage
{
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
